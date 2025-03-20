package com.example.recruitmentagencyandroid;

public class GlobalVariables {
    private static final String apiBaseUrl = "http://10.0.2.2:5045";
    private static final String defaultAuthSchema = "Bearer";

    public static String GetApiBaseUrl(){
        return apiBaseUrl;
    }

    public static String GetDefaultAuthSchema(){
        return defaultAuthSchema;
    }
}
