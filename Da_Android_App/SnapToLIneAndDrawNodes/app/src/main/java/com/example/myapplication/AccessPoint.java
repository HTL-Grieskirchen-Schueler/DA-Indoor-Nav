package com.example.myapplication;

public class AccessPoint {
    private String macAddress;
    private float left;
    private float top;

    public AccessPoint(String macAddress, float left, float top) {
        this.macAddress = macAddress;
        this.left = left;
        this.top = top;
    }

    public String getMacAddress() {
        return macAddress;
    }

    public float getLeft() {
        return left;
    }

    public float getTop() {
        return top;
    }
}
