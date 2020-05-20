package com.example.rest.web.serviceImpl;

import com.example.rest.db.dao.UserDAO;
import com.example.rest.db.daoModel.User;
import com.example.rest.web.service.UserService;
import org.hibernate.service.spi.ServiceException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
@Transactional
public class UserServiceImpl implements UserService {

    @Autowired
    private UserDAO userDAO;

    @Override
    public void addUser(User user) {
        User userFromDb = userDAO.findUserByUserLogin(user.getUserLogin());
        if (userFromDb == null) {
            userDAO.save(user.withIsAdmin(Boolean.FALSE));
        } else {
            throw new ServiceException("Login must be unique");
        }
    }

    @Override
    public void updateUser(int id, User user) {
        if (user.getUserLogin().equals("admin")) {
            throw new ServiceException("You can't modify administrator");
        }

        User userFromDb = userDAO.findUserByUserLogin(user.getUserLogin());
        if (userFromDb == null) {
            userFromDb = userDAO.findUserById(id);
            if ( userFromDb != null) {
                if (!user.getUserName().isEmpty() && user.getUserName() != null) {
                    user.setUserName(user.getUserName());
                }
                if (!user.getUserLastName().isEmpty() && user.getUserLastName() != null) {
                    user.setUserLastName(user.getUserLastName());
                }
                if (!user.getUserLogin().isEmpty() && user.getUserLogin() != null) {
                    user.setUserLogin(user.getUserLogin());
                }
                if (!user.getUserPassword().isEmpty() && user.getUserPassword() != null) {
                    user.setUserPassword(user.getUserPassword());
                }
                userDAO.save(user);
            } else {
                throw new ServiceException("User with given id is not existed");
            }
        } else {
            throw new ServiceException("Login must be unique");
        }
    }

    @Override
    public void deleteUser(int id) {
        if (userDAO.findUserById(id) != null) {

            if (userDAO.findUserById(id).getUserLogin().equals("admin")) {
                throw new ServiceException("You can't deleted administrator");
            }

            userDAO.deleteUserById(id);
        } else {
            throw new ServiceException("User with given id is not existed");
        }
    }

    @Override
    public User findUserById(int id) {
        return userDAO.findUserById(id);
    }

    @Override
    public List<User> findAll() {
        return userDAO.findAll();
    }
}
