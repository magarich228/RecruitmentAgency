package com.example.recruitmentagencyandroid;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.recruitmentagencyandroid.api.AuthApi;
import com.example.recruitmentagencyandroid.model.AuthResponse;
import com.example.recruitmentagencyandroid.model.LoginRequest;

import java.util.List;
import java.util.Map;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }

    public void LoginOnClick(View view) throws ApiException {
        AuthApi api = new AuthApi();
        api.setCustomBaseUrl(GlobalVariables.ApiBaseUrl);

        LoginRequest request = new LoginRequest();

        EditText phoneInput = findViewById(R.id.editTextPhone);
        EditText passInput = findViewById(R.id.editTextTextPassword);

        String phone = phoneInput.getText().toString();
        String pass = passInput.getText().toString();

        request.setPhoneNumber(phone);
        request.setPassword(pass);

        MainActivity context = this;

        ApiCallback<AuthResponse> callback = new ApiCallback<AuthResponse>() {
            @Override
            public void onFailure(ApiException e, int statusCode, Map<String, List<String>> responseHeaders) {
                runOnUiThread(() -> {
                    AlertDialog.Builder builder = new AlertDialog.Builder(context);

                    builder.setTitle("Ошибка при входе");
                    builder.setMessage(e.getMessage());

                    builder.setNegativeButton("Ок", (dialog, which) -> { });

                    AlertDialog dialog = builder.create();
                    dialog.show();
                });

                Log.e("Api error", e.toString());
            }

            @Override
            public void onSuccess(AuthResponse result, int statusCode, Map<String, List<String>> responseHeaders) {
                // TODO: set auth token
                runOnUiThread(() -> {
                    Toast.makeText(context, "Успешный вход", Toast.LENGTH_SHORT).show();
                });
            }

            @Override
            public void onUploadProgress(long bytesWritten, long contentLength, boolean done) {

            }

            @Override
            public void onDownloadProgress(long bytesRead, long contentLength, boolean done) {

            }
        };

        api.apiAuthLoginPostAsync(request, callback);

        Log.i("Login request", "Executing..");
    }

    public void GoToRegistrationOnClick(View view){
        Intent intent = new Intent(this, RegistrationActivity.class);
        startActivity(intent);
    }
}