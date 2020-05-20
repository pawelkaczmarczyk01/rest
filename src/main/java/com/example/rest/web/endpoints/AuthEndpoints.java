package com.example.rest.web.endpoints;

import com.example.rest.db.daoModel.User;
import com.example.rest.web.model.ChangePassword;
import com.example.rest.web.model.Login;
import com.example.rest.web.model.Register;
import com.example.rest.web.service.AuthService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/auth")
public class AuthEndpoints {

    @Autowired
    private AuthService authService;

    @RequestMapping(value = "/login", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.GET)
    public User login(@RequestBody Login login) {
        return authService.login(login);
    }

    @RequestMapping(value = "/register", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.POST)
    public void register(@RequestBody Register register) {
        authService.register(register);
    }

    @RequestMapping(value = "/changePassword", produces = MediaType.APPLICATION_JSON_VALUE, method = RequestMethod.PUT)
    public void changePassword(@RequestBody ChangePassword changePassword) {
        authService.changePassword(changePassword);
    }
}
