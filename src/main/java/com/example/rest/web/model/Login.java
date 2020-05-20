package com.example.rest.web.model;

public class Login {

    private String login;
    private String password;

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public Login withLogin(String login) {
        setLogin(login);
        return this;
    }

    public Login withPassword(String password) {
        setPassword(password);
        return this;
    }

}
