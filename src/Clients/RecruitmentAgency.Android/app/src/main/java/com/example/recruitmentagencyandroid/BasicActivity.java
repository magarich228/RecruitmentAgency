package com.example.recruitmentagencyandroid;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class BasicActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_basic);

        String userRole = UserContext.getUser()
                .getRole();

        TextView roleView = findViewById(R.id.roleNameView);
        roleView.setText(userRole);

        switch (userRole){
            case "Employee":
                break;

            case "Employer":
                break;

            case "Admin":
                break;

            default:
                break;
        }

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }

    public void ExitOnClick(View view)
    {
        /*
        Configuration
                .getDefaultApiClient()
                .removeAccessToken(); */
        UserContext.setUser(null, null);

        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }
}