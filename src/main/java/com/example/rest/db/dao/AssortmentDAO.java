package com.example.rest.db.dao;

import com.example.rest.db.daoModel.Assortment;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface AssortmentDAO extends JpaRepository<Assortment, Integer> {

    Assortment findAssortmentById(int id);

    void deleteAssortmentById(int id);
}
