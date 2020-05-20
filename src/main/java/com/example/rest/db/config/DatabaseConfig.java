package com.example.rest.db.config;

import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.transaction.annotation.EnableTransactionManagement;

@Configuration
@EnableTransactionManagement
@EntityScan(basePackages = "com.example.rest.db.daoModel")
@EnableJpaRepositories(basePackages = "com.example.rest.db.dao")
public class DatabaseConfig {
}
