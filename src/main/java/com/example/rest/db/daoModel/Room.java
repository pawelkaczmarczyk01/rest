package com.example.rest.db.daoModel;

import javax.persistence.*;

@Entity
@Table(name = "rooms")
public class Room {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    public Integer getId() {
        return id;
    }
    public void setId(Integer id) {
        this.id = id;
    }
    public Room withId(Integer id) {
        setId(id);
        return this;
    }

    @ManyToOne
    @JoinColumn(name = "hotelId")
    private Hotel hotelId;

    public Hotel getHotelId() {
        return hotelId;
    }
    public void setHotelId(Hotel hotelId) {
        this.hotelId = hotelId;
    }
    public Room withHotelId(Hotel hotelId) {
        setHotelId(hotelId);
        return this;
    }

    @ManyToOne
    @JoinColumn(name = "assortmentId")
    private Assortment assortmentId;

    public Assortment getAssortmentId() {
        return assortmentId;
    }
    public void setAssortmentId(Assortment assortmentId) {
        this.assortmentId = assortmentId;
    }
    public Room withAssortmentId(Assortment assortmentId) {
        setAssortmentId(assortmentId);
        return this;
    }

    @Column
    private String roomName;

    public String getRoomName() {
        return roomName;
    }
    public void setRoomName(String roomName) {
        this.roomName = roomName;
    }
    public Room withRoomName(String roomName) {
        setRoomName(roomName);
        return this;
    }

    @Column(length = 600)
    private String roomDescription;

    public String getRoomDescription() {
        return roomDescription;
    }
    public void setRoomDescription(String roomDescription) {
        this.roomDescription = roomDescription;
    }
    public Room withRoomDescription(String roomDescription) {
        setRoomDescription(roomDescription);
        return this;
    }

    @Column
    private Integer roomQuantityOfPeople;

    public Integer getRoomQuantityOfPeople() {
        return roomQuantityOfPeople;
    }
    public void setRoomQuantityOfPeople(Integer roomQuantityOfPeople) {
        this.roomQuantityOfPeople = roomQuantityOfPeople;
    }
    public Room withRoomQuantityOfPeople(Integer roomQuantityOfPeople) {
        setRoomQuantityOfPeople(roomQuantityOfPeople);
        return this;
    }

    @Column
    private Double roomPrice;

    public Double getRoomPrice() {
        return roomPrice;
    }
    public void setRoomPrice(Double roomPrice) {
        this.roomPrice = roomPrice;
    }
    public Room withRoomPrice(Double roomPrice) {
        setRoomPrice(roomPrice);
        return this;
    }

    @Column
    private String roomImagePath;

    public String getRoomImagePath() {
        return roomImagePath;
    }
    public void setRoomImagePath(String roomImagePath) {
        this.roomImagePath = roomImagePath;
    }
    public Room withRoomImagePath(String roomImagePath) {
        setRoomImagePath(roomImagePath);
        return this;
    }
}
