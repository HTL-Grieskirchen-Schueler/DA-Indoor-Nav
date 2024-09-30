package com.example.myapplication;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Matrix;
import android.graphics.Paint;
import android.graphics.Path;
import android.util.AttributeSet;
import android.view.GestureDetector;
import android.view.MotionEvent;
import android.view.ScaleGestureDetector;
import android.view.View;

import java.util.List;

public class NavigationView extends View {
    private List<NodeData> nodes;
    private List<AccessPoint> accessPoints;
    private Bitmap mapImage;
    private float touchX;
    private float touchY;
    private float orangeCircleX = -1;
    private float orangeCircleY = -1;
    private ScaleGestureDetector scaleGestureDetector;
    private GestureDetector gestureDetector;
    private Matrix matrix;
    private float scaleFactor = 1.0f;
    private List<NodeData> nodesSPF;
    private List<Integer> spfPath;
    private NodeData selectedNode;

    public NavigationView(Context context) {
        super(context);
        init();
    }
    public NavigationView(Context context, AttributeSet attrs) {
        super(context, attrs);
        init();
    }

    public NavigationView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        init();
    }
    private void init() {
        scaleGestureDetector = new ScaleGestureDetector(getContext(), new ScaleListener());
        gestureDetector = new GestureDetector(getContext(), new GestureListener());
        matrix = new Matrix();
    }
    public void setNodes(List<NodeData> nodes) {
        this.nodes = nodes;
        invalidate();
    }
    public void setAccessPoints(List<AccessPoint> accessPoints) {
        this.accessPoints = accessPoints;
        invalidate();
    }
    public void setMapImage(Bitmap mapImage) {
        this.mapImage = mapImage;
        invalidate();
    }
    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        canvas.save();
        canvas.scale(scaleFactor, scaleFactor);
        canvas.concat(matrix);

        if (mapImage != null) {
            canvas.drawBitmap(mapImage, 0, 0, null);
        }

        if (nodes != null) {
            for (NodeData nodeData : nodes) {
                drawNode(canvas, nodeData);
                drawEdges(canvas, nodeData);
                if (spfPath != null) {
                    drawPathBetweenLines(canvas);
                }
            }
        }

        if (accessPoints != null) {
            for (AccessPoint accessPoint : accessPoints) {
                drawRedCircle(canvas, accessPoint.getLeft(), accessPoint.getTop());
            }
        }

        drawRedCircle(canvas, touchX, touchY);

        draworangeCircle(canvas);

        drawLineToNearestNode(canvas);

        canvas.restore();
    }
    @Override
    public boolean onTouchEvent(MotionEvent event) {
        scaleGestureDetector.onTouchEvent(event);
        gestureDetector.onTouchEvent(event);
        return true;
    }
    private void drawRedCircle(Canvas canvas, float x, float y) {
        Paint paint = new Paint();
        paint.setColor(Color.RED);
        canvas.drawCircle(x, y, 10, paint);
    }
    private void drawNode(Canvas canvas, NodeData nodeData) {
        double absoluteX = nodeData.getRelativeX();
        double absoluteY = nodeData.getRelativeY();

        Paint paint = new Paint();
        paint.setColor(Color.GREEN);
        canvas.drawCircle((float) absoluteX, (float) absoluteY, 30, paint);
    }
    private void drawEdges(Canvas canvas, NodeData nodeData) {
        float startNodeX = (float) (nodeData.getRelativeX());
        float startNodeY = (float) (nodeData.getRelativeY());

        List<Integer> connectedNodes = nodeData.getConnectedNodes();
        if (connectedNodes != null && !connectedNodes.isEmpty()) {
            for (Integer connectedNodeId : connectedNodes) {
                NodeData connectedNode = findNodeById(connectedNodeId);
                if (connectedNode != null) {
                    float endNodeX = (float) (connectedNode.getRelativeX());
                    float endNodeY = (float) (connectedNode.getRelativeY());

                    Paint paint = new Paint();
                    paint.setColor(Color.BLUE);
                    paint.setStrokeWidth(2);
                    canvas.drawLine(startNodeX, startNodeY, endNodeX, endNodeY, paint);
                }
            }
        }
    }
    private NodeData findNodeById(int nodeId) {
        if (nodes != null) {
            for (NodeData node : nodes) {
                if (node.getId() == nodeId) {
                    return node;
                }
            }
        }
        return null;
    }
    public void setSelectedNode(NodeData pselectedNode) {
    this.selectedNode = pselectedNode;
    }
    public void setPathBetweenLines(List<NodeData> pnodes, List<Integer> mPath) {
    this.nodesSPF = pnodes;
    this.spfPath = mPath;

    }
    public void drawPathBetweenLines(Canvas canvas) {
        if (spfPath.size() < 2) {
            return;
        }

        Paint paint = new Paint();
        paint.setColor(Color.RED);
        paint.setStrokeWidth(5);
        paint.setStyle(Paint.Style.STROKE);

        Path pathLines = new Path();

        NodeData firstNode = nodesSPF.get(spfPath.get(0));
        pathLines.moveTo((float) firstNode.getRelativeX(), (float) firstNode.getRelativeY());

        for (int i = 1; i < spfPath.size(); i++) {
            NodeData node = nodesSPF.get(spfPath.get(i));
            pathLines.lineTo((float) node.getRelativeX(), (float) node.getRelativeY());
        }
        canvas.drawPath(pathLines, paint);
    }
    private class GestureListener extends GestureDetector.SimpleOnGestureListener{
        @Override
        public boolean onScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) {
            matrix.postTranslate(-distanceX, -distanceY);
            invalidate();
            return true;
        }
    }
    public void setorangeCirclePosition(float x, float y) {
        orangeCircleX = x;
        orangeCircleY = y;
        invalidate();
    }
    private void draworangeCircle(Canvas canvas) {
        Paint paint = new Paint();
        paint.setColor(Color.parseColor("#FFA500"));
        canvas.drawCircle(orangeCircleX, orangeCircleY, 70, paint);
        invalidate();
    }
    private class ScaleListener extends ScaleGestureDetector.SimpleOnScaleGestureListener {
        @Override
        public boolean onScale(ScaleGestureDetector detector) {
            scaleFactor *= detector.getScaleFactor();
            scaleFactor = Math.max(0.1f, Math.min(scaleFactor, 5.0f)); 
            matrix.setScale(scaleFactor, scaleFactor);
            invalidate();
            return true;
        }
    }
    private void drawLineToNearestNode(Canvas canvas) {
        if (orangeCircleX != -1 && orangeCircleY != -1 && nodes != null && !nodes.isEmpty()) {
            Paint paint = new Paint();
            paint.setColor(Color.BLACK);
            paint.setStrokeWidth(5);

            float orangeCircleCenterX = orangeCircleX;
            float orangeCircleCenterY = orangeCircleY;

            float minDistance = Float.MAX_VALUE;
            float nearestNodeX = -1;
            float nearestNodeY = -1;
            for (NodeData node : nodes) {
                float nodeX = (float) node.getRelativeX();
                float nodeY = (float) node.getRelativeY();
                float distance = (float) Math.sqrt(Math.pow(orangeCircleCenterX - nodeX, 2) + Math.pow(orangeCircleCenterY - nodeY, 2));
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestNodeX = nodeX;
                    nearestNodeY = nodeY;
                }
            }

            canvas.drawLine(orangeCircleCenterX, orangeCircleCenterY, nearestNodeX, nearestNodeY, paint);
        }
    }
}
