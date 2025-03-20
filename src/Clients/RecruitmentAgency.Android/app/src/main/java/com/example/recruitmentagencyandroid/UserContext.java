package com.example.recruitmentagencyandroid;

import com.example.recruitmentagencyandroid.model.UserInfo;

public class UserContext {
    private static volatile UserInfo userInfo;
    private static volatile String role;

    public static UserContextData getUser(){
        return new UserContextData(UserContext.userInfo, UserContext.role);
    }

    public static void setUser(UserInfo userInfo, String role)
    {
        UserContext.userInfo = userInfo;
        UserContext.role = role;
    }

    public static class UserContextData{

        public UserContextData(UserInfo userInfo, String role)
        {
            this.userInfo = userInfo;
            this.role = role;
        }

        private final UserInfo userInfo;
        private final String role;

        public UserInfo getUserInfo(){
            return userInfo;
        }

        public String getRole(){
            return role;
        }
    }
}
