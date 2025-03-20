/*
 * RecruitmentAgency.Api
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


package com.example.recruitmentagencyandroid.model;

import java.util.Objects;

import com.example.recruitmentagencyandroid.JSON;
import com.google.gson.TypeAdapter;
import com.google.gson.annotations.JsonAdapter;
import com.google.gson.annotations.SerializedName;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.util.Arrays;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParseException;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;

import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;


/**
 * ReportDto
 */
public class ReportDto {
  public static final String SERIALIZED_NAME_COMMISSION_INCOME = "commissionIncome";
  @SerializedName(SERIALIZED_NAME_COMMISSION_INCOME)
  @javax.annotation.Nullable
  private Double commissionIncome;

  public ReportDto() {
  }

  public ReportDto commissionIncome(@javax.annotation.Nullable Double commissionIncome) {
    this.commissionIncome = commissionIncome;
    return this;
  }

  /**
   * Get commissionIncome
   * @return commissionIncome
   */
  @javax.annotation.Nullable
  public Double getCommissionIncome() {
    return commissionIncome;
  }

  public void setCommissionIncome(@javax.annotation.Nullable Double commissionIncome) {
    this.commissionIncome = commissionIncome;
  }



  @Override
  public boolean equals(Object o) {
    if (this == o) {
      return true;
    }
    if (o == null || getClass() != o.getClass()) {
      return false;
    }
    ReportDto reportDto = (ReportDto) o;
    return Objects.equals(this.commissionIncome, reportDto.commissionIncome);
  }

  @Override
  public int hashCode() {
    return Objects.hash(commissionIncome);
  }

  @Override
  public String toString() {
    StringBuilder sb = new StringBuilder();
    sb.append("class ReportDto {\n");
    sb.append("    commissionIncome: ").append(toIndentedString(commissionIncome)).append("\n");
    sb.append("}");
    return sb.toString();
  }

  /**
   * Convert the given object to string with each line indented by 4 spaces
   * (except the first line).
   */
  private String toIndentedString(Object o) {
    if (o == null) {
      return "null";
    }
    return o.toString().replace("\n", "\n    ");
  }


  public static HashSet<String> openapiFields;
  public static HashSet<String> openapiRequiredFields;

  static {
    // a set of all properties/fields (JSON key names)
    openapiFields = new HashSet<String>();
    openapiFields.add("commissionIncome");

    // a set of required properties/fields (JSON key names)
    openapiRequiredFields = new HashSet<String>();
  }

  /**
   * Validates the JSON Element and throws an exception if issues found
   *
   * @param jsonElement JSON Element
   * @throws IOException if the JSON Element is invalid with respect to ReportDto
   */
  public static void validateJsonElement(JsonElement jsonElement) throws IOException {
      if (jsonElement == null) {
        if (!ReportDto.openapiRequiredFields.isEmpty()) { // has required fields but JSON element is null
          throw new IllegalArgumentException(String.format("The required field(s) %s in ReportDto is not found in the empty JSON string", ReportDto.openapiRequiredFields.toString()));
        }
      }

      Set<Map.Entry<String, JsonElement>> entries = jsonElement.getAsJsonObject().entrySet();
      // check to see if the JSON string contains additional fields
      for (Map.Entry<String, JsonElement> entry : entries) {
        if (!ReportDto.openapiFields.contains(entry.getKey())) {
          throw new IllegalArgumentException(String.format("The field `%s` in the JSON string is not defined in the `ReportDto` properties. JSON: %s", entry.getKey(), jsonElement.toString()));
        }
      }
        JsonObject jsonObj = jsonElement.getAsJsonObject();
  }

  public static class CustomTypeAdapterFactory implements TypeAdapterFactory {
    @SuppressWarnings("unchecked")
    @Override
    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
       if (!ReportDto.class.isAssignableFrom(type.getRawType())) {
         return null; // this class only serializes 'ReportDto' and its subtypes
       }
       final TypeAdapter<JsonElement> elementAdapter = gson.getAdapter(JsonElement.class);
       final TypeAdapter<ReportDto> thisAdapter
                        = gson.getDelegateAdapter(this, TypeToken.get(ReportDto.class));

       return (TypeAdapter<T>) new TypeAdapter<ReportDto>() {
           @Override
           public void write(JsonWriter out, ReportDto value) throws IOException {
             JsonObject obj = thisAdapter.toJsonTree(value).getAsJsonObject();
             elementAdapter.write(out, obj);
           }

           @Override
           public ReportDto read(JsonReader in) throws IOException {
             JsonElement jsonElement = elementAdapter.read(in);
             validateJsonElement(jsonElement);
             return thisAdapter.fromJsonTree(jsonElement);
           }

       }.nullSafe();
    }
  }

  /**
   * Create an instance of ReportDto given an JSON string
   *
   * @param jsonString JSON string
   * @return An instance of ReportDto
   * @throws IOException if the JSON string is invalid with respect to ReportDto
   */
  public static ReportDto fromJson(String jsonString) throws IOException {
    return JSON.getGson().fromJson(jsonString, ReportDto.class);
  }

  /**
   * Convert an instance of ReportDto to an JSON string
   *
   * @return JSON string
   */
  public String toJson() {
    return JSON.getGson().toJson(this);
  }
}

