package com.example.rest.web.serviceImpl;

import com.example.rest.db.dao.AssortmentDAO;
import com.example.rest.db.dao.HotelDAO;
import com.example.rest.db.dao.RoomDAO;
import com.example.rest.db.daoModel.Assortment;
import com.example.rest.db.daoModel.Hotel;
import com.example.rest.db.daoModel.Room;
import com.example.rest.web.model.RoomModel;
import com.example.rest.web.service.RoomService;
import org.hibernate.service.spi.ServiceException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.stream.Collectors;

@Service
@Transactional
public class RoomServiceImpl implements RoomService {

    @Autowired
    private RoomDAO roomDAO;
    @Autowired
    private HotelDAO hotelDAO;
    @Autowired
    private AssortmentDAO assortmentDAO;

    @Override
    public void addRoom(RoomModel roomModel) {
        if (hotelDAO.findHotelById(roomModel.getHotelId()) == null) {
            throw new ServiceException("Hotel with given id doesn't exist");
        }
        try {
            Assortment assortment = new Assortment()
                    .withRoomBathroom(roomModel.isRoomBathroom())
                    .withRoomDesk(roomModel.isRoomDesk())
                    .withRoomFridge(roomModel.isRoomFridge())
                    .withRoomSafe(roomModel.isRoomSafe())
                    .withRoomTv(roomModel.isRoomTv());
            assortment = assortmentDAO.save(assortment);

            Room room = new Room()
                    .withHotelId(new Hotel().withId(roomModel.getHotelId()))
                    .withAssortmentId(new Assortment().withId(assortment.getId()))
                    .withRoomDescription(roomModel.getRoomDescription())
                    .withRoomName(roomModel.getRoomName())
                    .withRoomPrice(roomModel.getRoomPrice())
                    .withRoomQuantityOfPeople(roomModel.getRoomQuantityOfPeople())
                    .withRoomImagePath(roomModel.getRoomImagePath());

            roomDAO.save(room);
        } catch (Exception e) {
            throw new ServiceException(e.getMessage(), e.getCause());
        }
    }

    @Override
    public void updateRoom(int id, RoomModel roomModel) {
        if (hotelDAO.findHotelById(roomModel.getHotelId()) == null) {
            throw new ServiceException("Hotel with given id doesn't exist");
        }
        try {
            Room roomFromDb = roomDAO.findRoomById(id);
            if (roomFromDb != null) {
                if (!roomModel.getRoomName().isEmpty() && roomModel.getRoomName() != null) {
                    roomFromDb.setRoomName(roomModel.getRoomName());
                }
                if (roomModel.getHotelId() != 0) {
                    roomFromDb.setHotelId(new Hotel().withId(roomModel.getHotelId()));
                }
                if (!roomModel.getRoomDescription().isEmpty() && roomModel.getRoomDescription() != null) {
                    roomFromDb.setRoomDescription(roomModel.getRoomDescription());
                }
                if (!roomModel.getRoomImagePath().isEmpty() && roomModel.getRoomImagePath() != null) {
                    roomFromDb.setRoomImagePath(roomModel.getRoomImagePath());
                }
                if (roomModel.getRoomQuantityOfPeople() != 0) {
                    roomFromDb.setRoomQuantityOfPeople(roomModel.getRoomQuantityOfPeople());
                }
                if (roomModel.getRoomPrice() != 0) {
                    roomFromDb.setRoomPrice(roomModel.getRoomPrice());
                }
                Assortment assortmentFromDb = assortmentDAO.findAssortmentById(roomFromDb.getAssortmentId().getId());
                if (roomModel.isRoomBathroom() != null) {
                    assortmentFromDb.setRoomBathroom(roomModel.isRoomBathroom());
                }
                if (roomModel.isRoomDesk() != null) {
                    assortmentFromDb.setRoomDesk(roomModel.isRoomDesk());
                }
                if (roomModel.isRoomFridge() != null) {
                    assortmentFromDb.setRoomFridge(roomModel.isRoomFridge());
                }
                if (roomModel.isRoomSafe() != null) {
                    assortmentFromDb.setRoomSafe(roomModel.isRoomSafe());
                }
                if (roomModel.isRoomTv() != null) {
                    assortmentFromDb.setRoomTv(roomModel.isRoomTv());
                }

                assortmentDAO.save(assortmentFromDb);
                roomDAO.save(roomFromDb);
            } else {
                throw new ServiceException("RoomModel with given id is not existed");
            }
        } catch (Exception e) {
            throw new ServiceException(e.getMessage(), e.getCause());
        }
    }

    @Override
    public void deleteRoom(int id) {
        Room roomFromDb = roomDAO.findRoomById(id);
        if (roomFromDb != null) {
            assortmentDAO.deleteAssortmentById(roomFromDb.getAssortmentId().getId());
            roomDAO.deleteRoomById(id);
        } else {
            throw new ServiceException("RoomModel with given id is not existed");
        }
    }

    @Override
    public Room findRoomById(int id) {
        return roomDAO.findRoomById(id);
    }

    @Override
    public List<Room> findAll() {
        return roomDAO.findAll();
    }

    @Override
    public List<Room> findAllRoomsByHotelId(int id) {
        return roomDAO.findAll().stream().filter(hotelId -> hotelId.getHotelId().getId().equals(id)).collect(Collectors.toList());
    }
}
