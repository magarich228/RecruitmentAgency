package com.example.recruitmentagencyandroid;

import static android.view.View.INVISIBLE;
import static android.view.View.VISIBLE;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.recruitmentagencyandroid.api.AuthApi;
import com.example.recruitmentagencyandroid.model.AuthResponse;
import com.example.recruitmentagencyandroid.model.RegisterEmployeeRequest;
import com.example.recruitmentagencyandroid.model.RegisterEmployerRequest;

import java.util.Objects;

public class RegistrationActivity extends AppCompatActivity {

    String[] roles = { "Работник", "Работодатель" };
    String selectedRole;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_registration);

        Spinner roleSpinner = findViewById(R.id.roleSpinner);

        ArrayAdapter<String> roleAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, roles);
        roleAdapter.setDropDownViewResource(android.R.layout.simple_spinner_item);

        roleSpinner.setAdapter(roleAdapter);

        AdapterView.OnItemSelectedListener roleSelectedListener = new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String role = (String)parent.getItemAtPosition(position);

                ConstraintLayout empLayout = findViewById(R.id.empConstraintLayout);
                ConstraintLayout emplrLayout = findViewById(R.id.emplrConstraintLayout);

                selectedRole = role;

                if (Objects.equals(role, "Работник")){
                    empLayout.setVisibility(VISIBLE);
                    emplrLayout.setVisibility(INVISIBLE);
                }
                else {
                    empLayout.setVisibility(INVISIBLE);
                    emplrLayout.setVisibility(VISIBLE);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        };

        roleSpinner.setOnItemSelectedListener(roleSelectedListener);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }

    public void RegistrationOnClick(View view) throws ApiException {
        AuthApi api = new AuthApi();

        EditText phoneInput = findViewById(R.id.editTextPhone);
        EditText passInput = findViewById(R.id.editTextTextPassword);
        EditText passConfirmInput = findViewById(R.id.editTextTextPassword3);

        EditText fullNameInput = findViewById(R.id.editTextText2);
        EditText resumeInput = findViewById(R.id.editTextTextMultiLine);

        EditText companyNameInput = findViewById(R.id.companyName);
        EditText companyDescInput = findViewById(R.id.companyDesc);
        EditText mainAddressInput = findViewById(R.id.mainAddress);

        String phone = phoneInput.getText().toString();
        String pass = passInput.getText().toString();
        String passConfirm = passConfirmInput.getText().toString();

        ApiCallback<AuthResponse> callback = AuthActivitiesShared.CreateCallback(this);

        if (selectedRole.equals("Работник")){
            String fullName = fullNameInput.getText().toString();
            String resume = resumeInput.getText().toString();

            RegisterEmployeeRequest request = new RegisterEmployeeRequest();

            request.setPhoneNumber(phone);
            request.setPassword(pass);
            request.setConfirmPassword(passConfirm);

            request.setFullName(fullName);
            request.setResume(resume);

            api.apiAuthRegisterAsEmployeePostAsync(request, callback);
        }
        else{
            String companyName = companyNameInput.getText().toString();
            String companyDesc = companyDescInput.getText().toString();
            String mainAddress = mainAddressInput.getText().toString();

            RegisterEmployerRequest request = new RegisterEmployerRequest();

            request.setPhoneNumber(phone);
            request.setPassword(pass);
            request.setConfirmPassword(passConfirm);

            request.setName(companyName);
            request.setDescription(companyDesc);
            request.setMainAddress(mainAddress);

            api.apiAuthRegisterAsEmployerPostAsync(request, callback);
        }

        Log.i("Register request", "Executing..");
    }

    public void GoToLoginOnClick(View view){
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }
}