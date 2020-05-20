package com.example.rest.web.serviceImpl;

import com.example.rest.db.dao.UserDAO;
import com.example.rest.db.daoModel.User;
import com.example.rest.web.model.ChangePassword;
import com.example.rest.web.model.Login;
import com.example.rest.web.model.Register;
import com.example.rest.web.service.AuthService;
import org.hibernate.service.spi.ServiceException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional
public class AuthServiceImpl implements AuthService {

    @Autowired
    private UserDAO userDAO;

    @Override
    public User login(Login login) {
        User userFromDb = userDAO.findUserByUserLogin(login.getLogin());
        if (userFromDb == null) {
            throw new ServiceException("Login or password is invalid");
        }
        if (userFromDb.getUserPassword().equals(login.getPassword())) {
            return userFromDb;
        } else {
            throw new ServiceException("Login or password is invalid");
        }
    }

    @Override
    public void register(Register register) {
        User userFromDao = userDAO.findUserByUserLogin(register.getUserLogin());
        if (userFromDao == null) {
            if (register.getUserPassword().equals(register.getUserConfirmPassword())) {
                User user = new User()
                        .withIsAdmin(Boolean.FALSE)
                        .withUserName(register.getUserName())
                        .withUserLastName(register.getUserLastName())
                        .withUserLogin(register. getUserLogin())
                        .withUserPassword(register.getUserPassword());
                userDAO.save(user);
                throw new ServiceException("User has been registered");
            } else {
                throw new ServiceException("Passwords is different");
            }
        } else {
            throw new ServiceException("User login already exist");
        }
    }

    @Override
    public void changePassword(ChangePassword changePassword) {
        User userFromDb = userDAO.findUserById(changePassword.getUserId());
        if (changePassword.getNewPassword().equals(changePassword.getConfirmPassword())) {
            if (userFromDb.getUserPassword().equals(changePassword.getOldPassword())) {
                if (!userFromDb.getUserPassword().equals(changePassword.getNewPassword())) {
                    userFromDb.setUserPassword(changePassword.getNewPassword());
                    userDAO.save(userFromDb);
                } else {
                    throw new ServiceException("New password can't be the same like old password");
                }
            } else {
                throw new ServiceException("Old password is wrong");
            }
        } else {
            throw new ServiceException("New passwords is different");
        }
    }
}
