package com.example.recruitmentagencyandroid;

import static android.view.View.INVISIBLE;
import static android.view.View.VISIBLE;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.Objects;

public class RegistrationActivity extends AppCompatActivity {

    String[] roles = { "Работник", "Работодатель" };

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

    public void RegistrationOnClick(View view){

    }

    public void GoToLoginOnClick(View view){
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }
}