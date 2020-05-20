package com.example.rest.web.service;

import com.example.rest.db.daoModel.User;

import java.util.List;

public interface UserService {

    void addUser(User user);

    void updateUser(int id, User user);

    void deleteUser(int id);

    User findUserById(int id);

    List<User> findAll();
}
