package com.example.myapplication;


import static androidx.constraintlayout.helper.widget.MotionEffect.TAG;

import android.content.Context;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Base64;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;

import okhttp3.OkHttpClient;

public class MainActivity extends AppCompatActivity {
    private Timer timer;
    private List<NodeData> nodes;
    private List<AccessPoint> accessPoints;
    private Bitmap mapImage;
    private int screenWidth;
    NavigationView customView;
    private int screenHight;
    private OkHttpClient client;
    String baseUrl = "https://3044-213-142-97-190.ngrok-free.app";
    private WifiManager wifiManager;
    private boolean test = true;
    private Context context;
    private Spinner RoomSpinner;
    private NodeData selectedNode;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        client = new OkHttpClient();
        context = getApplicationContext();
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        customView = findViewById(R.id.customView);
        RoomSpinner = findViewById(R.id.combobox);
        wifiManager = (WifiManager) getApplicationContext().getSystemService(Context.WIFI_SERVICE);
        if (!wifiManager.isWifiEnabled()) {
            wifiManager.setWifiEnabled(true);
        }
        screenHight = getScreenHeight() / (8 / 2);
        screenWidth = getScreenWidth();
        nodes = new ArrayList<>();
        accessPoints = new ArrayList<>();
        mapImage = null;
        NavigationView customView = findViewById(R.id.customView);
        if (test) {
            String nodeJsonData = "[" +
                    "{\"connectedNodes\":[]," +
                    "\"id\":0," +
                    "\"left\":1.768115090423266," +
                    "\"top\":1.2695410792124298," +
                    "\"name\":\"Room1\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":1," +
                    "\"left\":2.32281786388939," +
                    "\"top\":1.2452961399694067," +
                    "\"name\":\"Room2\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":2," +
                    "\"left\":2.68955505665981," +
                    "\"top\":1.2583511072541114," +
                    "\"name\":\"Room3\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":3," +
                    "\"left\":3.120650029777129," +
                    "\"top\":1.2490261306221793," +
                    "\"name\":\"Room4\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":4," +
                    "\"left\":3.4833010142715564," +
                    "\"top\":1.250891125948566," +
                    "\"name\":\"Room5\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":5," +
                    "\"left\":3.890900289801913," +
                    "\"top\":1.2452961399694067," +
                    "\"name\":\"Room6\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":6," +
                    "\"left\":4.3894176994731255," +
                    "\"top\":1.2546211166013386," +
                    "\"name\":\"Room7\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":7," +
                    "\"left\":4.401676324301105," +
                    "\"top\":2.808162223481203," +
                    "\"name\":\"Room8\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":8," +
                    "\"left\":1.7395116324913118," +
                    "\"top\":2.8007022421756576," +
                    "\"name\":\"Room9\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":9," +
                    "\"left\":2.3769601235463043," +
                    "\"top\":1.9931592658503499," +
                    "\"name\":\"Room10\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":10," +
                    "\"left\":2.8407447628715716," +
                    "\"top\":1.9838342892184178," +
                    "\"name\":\"Room11\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":11," +
                    "\"left\":3.2432362780569353," +
                    "\"top\":2.0062142331350548," +
                    "\"name\":\"Room12\"}," +

                    "{\"connectedNodes\":[]," +
                    "\"id\":12," +
                    "\"left\":3.9961201529087464," +
                    "\"top\":2.0062142331350548," +
                    "\"name\":\"Room13\"}," +

                    "{\"connectedNodes\":[16,16,16,16,16,16,16,16]," +
                    "\"id\":13," +
                    "\"left\":0.3442291666666666," +
                    "\"top\":0.29956810588537847," +
                    "\"name\":\"links oben\"}," +

                    "{\"connectedNodes\":[15,15,15,15,15,15,15,15]," +
                    "\"id\":14," +
                    "\"left\":0.8634291666666666," +
                    "\"top\":0.2952990559574845," +
                    "\"name\":\"Room14\"}," +

                    "{\"connectedNodes\":[14,14,14,14,14,14,14,16,14,16,16,16,16,16,16,16]," +
                    "\"id\":15," +
                    "\"left\":0.8634291666666666," +
                    "\"top\":0.6389575751529487," +
                    "\"name\":\"rechts unten\"}," +

                    "{\"connectedNodes\":[13,13,13,13,15,15,15,15,13,13,15,15,13,15,13,15]," +
                    "\"id\":16," +
                    "\"left\":0.34482916666666663," +
                    "\"top\":0.6385306701601593," +
                    "\"name\":\"Room15\"}" +
                    "]";


            String accessPointJsonData = "";


            // Assume MapData is some predefined base64 string
            String imageJsonData = "somebase64String";

            handleJsonData(nodeJsonData, accessPointJsonData, imageJsonData);
        } else {
            String nodeUrl = baseUrl + "/ImageCrl/nodes";
            String accesspointUrl = baseUrl + "/ImageCrl/accesspoints";
            String imageUrl = baseUrl + "/ImageCrl/mapData";

            new DownloadDataTask().execute(nodeUrl, accesspointUrl, imageUrl);
        }
        String base64String = Base64Handler.readBase64FromFile(this);
        mapImage = Base64Handler.decodeBase64(base64String);
        customView.setMapImage(mapImage);
        startPeriodicScan();
        populateSpinner();
        client = new OkHttpClient();
        RoomSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                NodeData sNode = nodes.get(position);
                selectedNode = sNode;
                customView.setSelectedNode(selectedNode);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                NodeData sNode = nodes.get(0);
                selectedNode = sNode;
                customView.setSelectedNode(selectedNode);
            }
        });
    }
    private void populateSpinner() {

        List<String> nodeNames = new ArrayList<>();
        for (NodeData node : nodes) {
            nodeNames.add(node.getName());
        }

        ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, nodeNames);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        RoomSpinner.setAdapter(adapter);
    }
    private void startPeriodicScan() {
        timer = new Timer();
        timer.schedule(new TimerTask() {
            @Override
            public void run() {
                scanForAccessPoints();
            }
        }, 10000, 10000);
    }
    private void scanForAccessPoints() {
        if (ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{android.Manifest.permission.ACCESS_FINE_LOCATION}, 111);
            return;
        }
        WifiInfo wifiInfo = wifiManager.getConnectionInfo();
        List<ScanResult> scanResults = wifiManager.getScanResults();

        StringBuilder parametersBuilder = new StringBuilder();
        for (ScanResult scanResult : scanResults) {
            String bssid = scanResult.SSID;
            int frequency = scanResult.frequency;
            int rssi = scanResult.level;

            double distance = calculateDistance(frequency, rssi);
            distance = Math.round(distance * 100.0) / 100.0;

            parametersBuilder.append(bssid).append(";").append(distance).append(",");
        }

        if (parametersBuilder.length() > 0) {
            parametersBuilder.deleteCharAt(parametersBuilder.length() - 1);
        }

        String parameters = parametersBuilder.toString();
        callEndpoint(parameters);
    }
    private void callEndpoint(String parameters) {
        Map<String, String> data = new HashMap<>();
        data.put("parameters", parameters);

        new AsyncTaskForBackendAccess(context, data).execute();
    }
    private double calculateDistance(int frequency, int rssi) {
        double referenceSignalLevel = -45.0;
        double propagationExponent = 2.7;
        double distance = Math.pow(10, (referenceSignalLevel - rssi) / (10 * propagationExponent));

        distance = Math.sqrt(distance * distance - 1);
        double frequencyAdjustment = 27.55 - (20 * Math.log10(frequency));
        distance = distance * Math.pow(10, (frequencyAdjustment - referenceSignalLevel) / (20 * propagationExponent));

        return distance;
    }
    private class DownloadDataTask extends AsyncTask<String, Void, List<String>> {
        @Override
        protected List<String> doInBackground(String... urls) {
            List<String> result = new ArrayList<>();

            for (String url : urls) {
                String jsonData = downloadJsonData(url);
                if (jsonData != null) {
                    result.add(jsonData);
                } else {
                    result.add("");
                }
            }

            return result;
        }

        @Override
        protected void onPostExecute(List<String> result) {
            if (result.size() == 3) {
                handleJsonData(result.get(0), result.get(1), result.get(2));
            } else {
                Toast.makeText(MainActivity.this, "Failed to download data", Toast.LENGTH_SHORT).show();
            }
        }
    }
    private String downloadJsonData(String jsonUrl) {
        try {
            URL url = new URL(jsonUrl);
            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

            try {
                InputStream inputStream = urlConnection.getInputStream();
                BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
                StringBuilder stringBuilder = new StringBuilder();
                String line;

                while ((line = reader.readLine()) != null) {
                    stringBuilder.append(line).append("\n");
                }

                inputStream.close();

                return stringBuilder.toString();
            } finally {
                urlConnection.disconnect();
            }
        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }
    private void handleJsonData(String nodeJsonData, String accessPointJsonData, String imageBase64Data) {
        try {
            nodes = parseNodeData(nodeJsonData);
            accessPoints = parseAccessPointData(accessPointJsonData);
            mapImage = decodeBase64Image(imageBase64Data);
        } catch (JSONException e) {
            e.printStackTrace();
            Toast.makeText(MainActivity.this, "Failed to parse JSON data", Toast.LENGTH_SHORT).show();
        }
        customView.setNodes(nodes);
        customView.setAccessPoints(accessPoints);
        customView.setMapImage(mapImage);
    }
    private Bitmap decodeBase64Image(String base64Image) {
        byte[] decodedBytes = Base64.decode(base64Image, Base64.DEFAULT);
        return BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.length);
    }
    private List<NodeData> parseNodeData(String nodeJsonData) throws JSONException {
        List<NodeData> nodeDataList = new ArrayList<>();
        JSONArray jsonArray = new JSONArray(nodeJsonData);

        for (int i = 0; i < jsonArray.length(); i++) {
            JSONObject jsonNode = jsonArray.getJSONObject(i);

            int id = jsonNode.getInt("id");
            double left = jsonNode.getDouble("left");
            double top = jsonNode.getDouble("top");
            String name = jsonNode.getString("name");
            JSONArray connectedNodesArray = jsonNode.getJSONArray("connectedNodes");

            List<Integer> connectedNodes = new ArrayList<>();
            for (int j = 0; j < connectedNodesArray.length(); j++) {
                connectedNodes.add(connectedNodesArray.getInt(j));
            }

            NodeData nodeData = new NodeData(id, left * screenWidth, top * screenHight, connectedNodes, name);
            nodeDataList.add(nodeData);
        }

        return nodeDataList;
    }
    private int getScreenWidth() {
        DisplayMetrics displayMetrics = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(displayMetrics);
        return displayMetrics.widthPixels;
    }
    private int getScreenHeight() {
        DisplayMetrics displayMetrics = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(displayMetrics);
        return displayMetrics.heightPixels;
    }
    private List<AccessPoint> parseAccessPointData(String accessPointJsonData) throws JSONException {
        return null;
    }
    private class AsyncTaskForBackendAccess extends AsyncTask<Void, Void, String> {
        private Context mContext;
        private Map<String, String> mData;
        private List<Integer> mPath;

        public AsyncTaskForBackendAccess(Context context, Map<String, String> data) {
            mContext = context;
            mData = data;
            mPath = new ArrayList<>();
        }
        @Override
        protected String doInBackground(Void... voids) {
            String positionJson = "";
            try {
                HttpURLConnection positionConnection = getPostConnection(mData, baseUrl + "/getPosition");
                int positionResponseCode = positionConnection.getResponseCode();
                if (positionResponseCode == HttpURLConnection.HTTP_OK) {
                    BufferedReader reader = new BufferedReader(new InputStreamReader(positionConnection.getInputStream()));
                    positionJson = readResponseStream(reader);
                    positionJson = positionJson.replaceAll("[^\\d.,]", "");
                    reader.close();
                    positionConnection.disconnect();

                    String[] coordinates = positionJson.split(",");
                    if (coordinates.length == 2) {
                        float x = Float.parseFloat(coordinates[0].trim()) * screenWidth;
                        float y = Float.parseFloat(coordinates[1].trim()) * screenHight;

                        customView.setorangeCirclePosition(x, y);

                        NodeData nearestNode = setNearestNode(x, y);
                        if (nearestNode != null) {
                            mPath = callSPFEndpoint(nearestNode.getId(), selectedNode.getId());
                        }
                    }
                }
            } catch (IOException e) {
            }

            return positionJson;
        }

        private List<Integer> callSPFEndpoint(int nodeId1, int nodeId2) {
            List<Integer> path = new ArrayList<>();

            try {
                String endpoint = baseUrl + "/spf?id1=" + nodeId1 + "&id2=" + nodeId2;
                URL url = new URL(endpoint);

                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                urlConnection.setRequestMethod("GET");

                BufferedReader in = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                StringBuilder response = new StringBuilder();
                String line;
                while ((line = in.readLine()) != null) {
                    response.append(line);
                }
                in.close();

                JSONArray jsonArray = new JSONArray(response.toString());
                for (int i = 0; i < jsonArray.length(); i++) {
                    path.add(jsonArray.getInt(i));
                }
            } catch (IOException | JSONException e) {
                e.printStackTrace();
            }

            return path;
        }


        @Override
        protected void onPostExecute(String s) {
            Log.d(TAG, "Response: " + s);
            s = s.replaceAll("[^\\d.,]", "");
            Toast.makeText(mContext, "POST request completed", Toast.LENGTH_SHORT).show();
            String[] coordinates = s.split(",");

            if (coordinates.length == 2) {
                float x = Float.parseFloat(coordinates[0].trim()) * screenWidth;
                float y = Float.parseFloat(coordinates[1].trim()) * screenHight;

                customView = findViewById(R.id.customView);
                customView.setorangeCirclePosition(x, y);
                customView.setPathBetweenLines(nodes,mPath);

            }
        }

        private HttpURLConnection getPostConnection(Map<String, String> data, String url) throws IOException {
            JSONObject postParams = new JSONObject(data);
            String body = postParams.toString();

            HttpURLConnection connection = (HttpURLConnection) new URL(url).openConnection();
            connection.setDoOutput(true);
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setFixedLengthStreamingMode(body.getBytes().length);

            OutputStream outputStream = connection.getOutputStream();
            outputStream.write(body.getBytes());
            outputStream.flush();

            return connection;
        }

        private String readResponseStream(BufferedReader reader) throws IOException {
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                stringBuilder.append(line);
            }
            return stringBuilder.toString();
        }

        private NodeData setNearestNode(float x, float y) {
            NodeData nearestNode = null;
            double minDistance = Double.MAX_VALUE;

            for (NodeData node : nodes) {
                double nodeX = node.getRelativeX() / screenWidth;
                double nodeY = node.getRelativeY() / screenHight;

                double distance = Math.sqrt(Math.pow(nodeX - x, 2) + Math.pow(nodeY - y, 2));
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestNode = node;
                }
            }
            return nearestNode;
        }

/*        private List<Integer> callSPFEndpoint(int nodeId1, int nodeId2) {
            List<Integer> path = new ArrayList<>();

            try {
                String endpoint = baseUrl + "/spf?id1=" + nodeId1 + "&id2=" + nodeId2;
                URL url = new URL(endpoint);

                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                urlConnection.setRequestMethod("GET");

                BufferedReader in = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                StringBuilder response = new StringBuilder();
                String line;
                while ((line = in.readLine()) != null) {
                    response.append(line);
                }
                in.close();

                JSONArray jsonArray = new JSONArray(response.toString());
                for (int i = 0; i < jsonArray.length(); i++) {
                    path.add(jsonArray.getInt(i));
                }
            } catch (IOException | JSONException e) {
                e.printStackTrace();
            }

            return path;
        }*/
    }

}
