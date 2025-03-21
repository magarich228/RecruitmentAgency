package com.example.recruitmentagencyandroid;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

import androidx.appcompat.app.AlertDialog;

import com.example.recruitmentagencyandroid.api.AuthApi;
import com.example.recruitmentagencyandroid.model.AuthResponse;
import com.example.recruitmentagencyandroid.model.UserInfo;

import java.util.List;
import java.util.Map;

public class AuthActivitiesShared {
    public static ApiCallback<AuthResponse> CreateCallback(Activity context){
        ApiCallback<AuthResponse> callback = new ApiCallback<AuthResponse>() {
            @Override
            public void onFailure(ApiException e, int statusCode, Map<String, List<String>> responseHeaders) {
                ShowError("Ошибка при входе", e);
                Log.e("Api error", e.toString());
            }

            @Override
            public void onSuccess(AuthResponse result, int statusCode, Map<String, List<String>> responseHeaders) {
                Configuration.getDefaultApiClient()
                        .setAccessToken(result.getToken());

                AuthApi test = new AuthApi();
                UserInfo userInfo;

                try {
                    userInfo = test.apiAuthMeGet();
                } catch (ApiException e) {
                    ShowError("ошибка при получении данных пользователя", e);

                    return;
                }

                UserContext.setUser(userInfo, result.getRole());

                context.runOnUiThread(() -> {
                    Toast.makeText(context, "Успешный вход", Toast.LENGTH_SHORT).show();

                    Intent intent = new Intent(context, BasicActivity.class);
                    context.startActivity(intent);
                });
            }

            @Override
            public void onUploadProgress(long bytesWritten, long contentLength, boolean done) {

            }

            @Override
            public void onDownloadProgress(long bytesRead, long contentLength, boolean done) {

            }

            private void ShowError(String title, Exception e){
                context.runOnUiThread(() -> {
                    AlertDialog.Builder builder = new AlertDialog.Builder(context);

                    builder.setTitle(title);
                    builder.setMessage(e.getMessage());

                    builder.setNegativeButton("Ок", (dialog, which) -> { });

                    AlertDialog dialog = builder.create();
                    dialog.show();
                });
            }
        };

        return callback;
    }
}
