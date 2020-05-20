package com.example.rest.db.config;

import com.example.rest.db.dao.UserDAO;
import com.example.rest.db.daoModel.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import javax.annotation.PostConstruct;

@Component
public class DataInit {

    @Autowired
    private UserDAO userDAO;

    @PostConstruct
    public void init() {
        if(userDAO.findUserByUserLogin("admin") == null) {
            User user = new User()
                    .withIsAdmin(true)
                    .withUserName("admin")
                    .withUserLastName("admin")
                    .withUserLogin("admin")
                    .withUserPassword("admin1234#");
            userDAO.save(user);
        }
    }
}
