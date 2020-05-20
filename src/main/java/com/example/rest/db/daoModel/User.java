package com.example.rest.db.daoModel;

import javax.persistence.*;

@Entity
@Table(name = "users", uniqueConstraints = @UniqueConstraint(columnNames = "userLogin"))
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    public Integer getId() {
        return id;
    }
    public void setId(Integer id) {
        this.id = id;
    }
    public User withId(Integer id) {
        setId(id);
        return this;
    }

    @Column
    private String userName;

    public String getUserName() {
        return userName;
    }
    public void setUserName(String userName) {
        this.userName = userName;
    }
    public User withUserName(String userName) {
        setUserName(userName);
        return this;
    }

    @Column
    private String userLastName;

    public String getUserLastName() {
        return userLastName;
    }
    public void setUserLastName(String userLastName) {
        this.userLastName = userLastName;
    }
    public User withUserLastName(String userLastName) {
        setUserLastName(userLastName);
        return this;
    }

    @Column
    private String userLogin;

    public String getUserLogin() {
        return userLogin;
    }
    public void setUserLogin(String userLogin) {
        this.userLogin = userLogin;
    }
    public User withUserLogin(String userLogin) {
        setUserLogin(userLogin);
        return this;
    }

    @Column
    private String userPassword;

    public String getUserPassword() {
        return userPassword;
    }
    public void setUserPassword(String userPassword) {
        this.userPassword = userPassword;
    }
    public User withUserPassword(String userPassword) {
        setUserPassword(userPassword);
        return this;
    }

    @Column
    private boolean isAdmin;

    public boolean getIsAdmin() { return isAdmin; }
    public void setIsAdmin(boolean isAdmin) { this.isAdmin = isAdmin; }
    public User withIsAdmin(boolean isAdmin) {
        setIsAdmin(isAdmin);
        return this;
    }
}
