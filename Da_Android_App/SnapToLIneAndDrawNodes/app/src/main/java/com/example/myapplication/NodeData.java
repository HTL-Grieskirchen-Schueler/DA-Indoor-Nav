package com.example.myapplication;

import java.util.List;

public class NodeData {
    private int id;
    private double relativeX;
    private double relativeY;
    private List<Integer> connectedNodes;
    private String name;

    public NodeData(int id, double relativeX, double relativeY, List<Integer> connectedNodes,String name) {
        this.id = id;
        this.relativeX = relativeX;
        this.relativeY = relativeY;
        this.connectedNodes = connectedNodes;
        this.name = name;
    }
    public String getName(){return name;}

    public int getId() {
        return id;
    }

    public double getRelativeX() {
        return relativeX;
    }

    public double getRelativeY() {
        return relativeY;
    }

    public List<Integer> getConnectedNodes() {
        return connectedNodes;
    }
}
