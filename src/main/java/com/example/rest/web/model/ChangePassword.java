package com.example.rest.web.model;

public class ChangePassword {

    private int userId;
    private String oldPassword;
    private String newPassword;
    private String confirmPassword;

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public String getOldPassword() {
        return oldPassword;
    }

    public void setOldPassword(String oldPassword) {
        this.oldPassword = oldPassword;
    }

    public String getNewPassword() {
        return newPassword;
    }

    public void setNewPassword(String newPassword) {
        this.newPassword = newPassword;
    }

    public String getConfirmPassword() {
        return confirmPassword;
    }

    public void setConfirmPassword(String confirmPassword) {
        this.confirmPassword = confirmPassword;
    }

    public ChangePassword withUserId(int userId) {
        setUserId(userId);
        return this;
    }

    public ChangePassword withOldPassword(String oldPassword) {
        setOldPassword(oldPassword);
        return this;
    }

    public ChangePassword withNewPassword(String newPassword) {
        setNewPassword(newPassword);
        return this;
    }

    public ChangePassword withConfirmPassword(String confirmPassword) {
        setConfirmPassword(confirmPassword);
        return this;
    }
}
