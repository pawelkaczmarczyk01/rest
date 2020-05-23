package com.example.rest.web.model;


public class RoomModel {

    private String roomDescription;
    private String roomImagePath;
    private String roomName;
    private double roomPrice;
    private int roomQuantityOfPeople;
    private int hotelId;
    private Boolean roomBathroom;
    private Boolean roomDesk;
    private Boolean roomFridge;
    private Boolean roomSafe;
    private Boolean roomTv;

    public String getRoomDescription() {
        return roomDescription;
    }

    public void setRoomDescription(String roomDescription) {
        this.roomDescription = roomDescription;
    }

    public String getRoomImagePath() {
        return roomImagePath;
    }

    public void setRoomImagePath(String roomImagePath) {
        this.roomImagePath = roomImagePath;
    }

    public String getRoomName() {
        return roomName;
    }

    public void setRoomName(String roomName) {
        this.roomName = roomName;
    }

    public double getRoomPrice() {
        return roomPrice;
    }

    public void setRoomPrice(double roomPrice) {
        this.roomPrice = roomPrice;
    }

    public int getRoomQuantityOfPeople() {
        return roomQuantityOfPeople;
    }

    public void setRoomQuantityOfPeople(int roomQuantityOfPeople) {
        this.roomQuantityOfPeople = roomQuantityOfPeople;
    }

    public int getHotelId() {
        return hotelId;
    }

    public void setHotelId(int hotelId) {
        this.hotelId = hotelId;
    }

    public Boolean isRoomBathroom() {
        return roomBathroom;
    }

    public void setRoomBathroom(Boolean roomBathroom) {
        this.roomBathroom = roomBathroom;
    }

    public Boolean isRoomDesk() {
        return roomDesk;
    }

    public void setRoomDesk(Boolean roomDesk) {
        this.roomDesk = roomDesk;
    }

    public Boolean isRoomFridge() {
        return roomFridge;
    }

    public void setRoomFridge(Boolean roomFridge) {
        this.roomFridge = roomFridge;
    }

    public Boolean isRoomSafe() {
        return roomSafe;
    }

    public void setRoomSafe(Boolean roomSafe) {
        this.roomSafe = roomSafe;
    }

    public Boolean isRoomTv() {
        return roomTv;
    }

    public void setRoomTv(Boolean roomTv) {
        this.roomTv = roomTv;
    }
}
