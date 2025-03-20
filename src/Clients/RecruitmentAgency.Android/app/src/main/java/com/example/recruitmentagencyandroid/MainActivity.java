package com.example.recruitmentagencyandroid;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;

import androidx.activity.EdgeToEdge;
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

        Configuration
                .getDefaultApiClient()
                .setBasePath(GlobalVariables.GetApiBaseUrl());

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }

    public void LoginOnClick(View view) throws ApiException {
        AuthApi api = new AuthApi();

        LoginRequest request = new LoginRequest();

        EditText phoneInput = findViewById(R.id.editTextPhone);
        EditText passInput = findViewById(R.id.editTextTextPassword);

        String phone = phoneInput.getText().toString();
        String pass = passInput.getText().toString();

        request.setPhoneNumber(phone);
        request.setPassword(pass);

        ApiCallback<AuthResponse> callback = AuthActivitiesShared.CreateCallback(this);

        api.apiAuthLoginPostAsync(request, callback);

        Log.i("Login request", "Executing..");
    }

    public void GoToRegistrationOnClick(View view){
        Intent intent = new Intent(this, RegistrationActivity.class);
        startActivity(intent);
    }
}