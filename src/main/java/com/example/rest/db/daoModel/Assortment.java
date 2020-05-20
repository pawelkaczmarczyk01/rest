package com.example.rest.db.daoModel;

import javax.persistence.*;

@Entity
@Table(name = "assortments")
public class Assortment {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    public Integer getId() {
        return id;
    }
    public void setId(Integer id) {
        this.id = id;
    }
    public Assortment withId(Integer id) {
        setId(id);
        return this;
    }

    @Column
    private boolean roomTv;

    public boolean getRoomTv() {
        return roomTv;
    }
    public void setRoomTv(boolean roomTv) {
        this.roomTv = roomTv;
    }
    public Assortment withRoomTv(boolean roomTv) {
        setRoomTv(roomTv);
        return this;
    }

    @Column
    private boolean roomBathroom;

    public boolean getRoomBathroom() {
        return roomBathroom;
    }
    public void setRoomBathroom(boolean roomBathroom) {
        this.roomBathroom = roomBathroom;
    }
    public Assortment withRoomBathroom(boolean roomBathroom) {
        setRoomBathroom(roomBathroom);
        return this;
    }

    @Column
    private boolean roomDesk;

    public boolean getRoomDesk() {
        return roomDesk;
    }
    public void setRoomDesk(boolean roomDesk) {
        this.roomDesk = roomDesk;
    }
    public Assortment withRoomDesk(boolean roomDesk) {
        setRoomDesk(roomDesk);
        return this;
    }

    @Column
    private boolean roomSafe;

    public boolean getRoomSafe() {
        return roomSafe;
    }
    public void setRoomSafe(boolean roomSafe) {
        this.roomSafe = roomSafe;
    }
    public Assortment withRoomSafe(boolean roomSafe) {
        setRoomSafe(roomSafe);
        return this;
    }

    @Column
    private boolean roomFridge;

    public boolean getRoomFridge() {
        return roomFridge;
    }
    public void setRoomFridge(boolean roomFridge) {
        this.roomFridge = roomFridge;
    }
    public Assortment withRoomFridge(boolean roomFridge) {
        setRoomFridge(roomFridge);
        return this;
    }
}
