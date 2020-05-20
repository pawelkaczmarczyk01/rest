package com.example.rest.web.model;

public class Register {

    private String userName;
    private String userLastName;
    private String userLogin;
    private String userPassword;
    private String userConfirmPassword;

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getUserLastName() {
        return userLastName;
    }

    public void setUserLastName(String userLastName) {
        this.userLastName = userLastName;
    }

    public String getUserLogin() {
        return userLogin;
    }

    public void setUserLogin(String userLogin) {
        this.userLogin = userLogin;
    }

    public String getUserPassword() {
        return userPassword;
    }

    public void setUserPassword(String userPassword) {
        this.userPassword = userPassword;
    }

    public String getUserConfirmPassword() {
        return userConfirmPassword;
    }

    public void setUserConfirmPassword(String userConfirmPassword) {
        this.userConfirmPassword = userConfirmPassword;
    }

    public Register withUserName(String userName) {
        setUserName(userName);
        return this;
    }

    public Register withUserLastName(String userLastName) {
        setUserLastName(userLastName);
        return this;
    }

    public Register withUserLogin(String userLogin) {
        setUserLogin(userLogin);
        return this;
    }

    public Register withUserPassword(String userPassword) {
        setUserPassword(userPassword);
        return this;
    }

    public Register withUserConfirmPassword(String userConfirmPassword) {
        setUserConfirmPassword(userConfirmPassword);
        return this;
    }
}
