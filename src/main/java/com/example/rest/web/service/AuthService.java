package com.example.rest.web.service;

import com.example.rest.db.daoModel.User;
import com.example.rest.web.model.ChangePassword;
import com.example.rest.web.model.Login;
import com.example.rest.web.model.Register;

public interface AuthService {

    User login(Login login);

    void register(Register register);

    void changePassword(ChangePassword changePassword);
}
