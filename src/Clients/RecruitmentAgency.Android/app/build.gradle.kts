plugins {
    alias(libs.plugins.android.application)
}

android {
    namespace = "com.example.recruitmentagencyandroid"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.example.recruitmentagencyandroid"
        minSdk = 26
        targetSdk = 35
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }

    packaging {
        resources {
            excludes += setOf(
                "META-INF/NOTICE.md",
                "META-INF/LICENSE.md",
                "META-INF/INDEX.LIST",
                "META-INF/DEPENDENCIES"
            )
        }
    }
}

dependencies {

    implementation(libs.appcompat)
    implementation(libs.material)
    implementation(libs.activity)
    implementation(libs.constraintlayout)
    testImplementation(libs.junit)
    androidTestImplementation(libs.ext.junit)
    androidTestImplementation(libs.espresso.core)

    implementation(libs.swagger.annotations)
    implementation(libs.jsr305)
    implementation(libs.okhttp)
    implementation(libs.logging.interceptor)
    implementation(libs.gson)
    implementation(libs.gson.fire)
    implementation(libs.jakarta.ws.rs.api)
    implementation(libs.jackson.databind.nullable)
    implementation(libs.commons.lang3)
    implementation(libs.jakarta.annotation.api)
}