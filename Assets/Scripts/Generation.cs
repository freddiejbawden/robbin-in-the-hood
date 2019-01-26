using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {
    [SerializeField] private int size = 10;
    [SerializeField] private GameObject empty;
    [SerializeField] private GameObject corridorCorner;
    [SerializeField] private GameObject corridorCross;
    [SerializeField] private GameObject corridorStraight;
    [SerializeField] private GameObject corridorT;
    [SerializeField] private GameObject room1;
    [SerializeField] private GameObject room2Corner;
    [SerializeField] private GameObject room2Straight;
    [SerializeField] private GameObject room3;
    [SerializeField] private GameObject room4;
    private GameObject[,] rooms;

    private void Start() {
        rooms = new GameObject[size, size];

        createGrid();
        criticalPath();
        nonCritical();
        deleteEmpty();
        layoutRooms();
        populateRooms();
        //debugCritical();
    }

    private void debugCritical() {
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                if(rooms[i, j].GetComponent<Room>().getCritical()) {
                    Transform obj = rooms[i, j].transform.GetChild(0);
                    
                    for(int num = 0; num < obj.childCount; num++) {
                        obj.GetChild(num).GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

    private void deleteEmpty() {
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                if(!rooms[i, j].GetComponent<Room>().getAttached()) {
                    Destroy(rooms[i, j]);
                }
            }
        }

        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                MapManager.roomCentres.Add(rooms[i, j].transform.position);
            }
        }
    }

    private void createGrid() {
        //Place initial empty gameobjects in correct spots for rooms;
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                rooms[i, j] = Instantiate(empty, new Vector3(i * -8.5f, 0.0f, j * 8.5f), Quaternion.identity, transform);
                rooms[i, j].transform.name = "(" + i + ", " + j + ")" ;
            }
        }
    }

    private void criticalPath() {
        //Creating a path the player can definitely follow to traverse the house
        int i = Random.Range(0, size);
        int j = 0;

        //Start room
        rooms[i, j].GetComponent<Room>().setCritical(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInitialPosition>().initialPos(new Vector3(i * -8.5f, 1.0f, 0.0f));

        int chosen = -1;
       
        while(j < size) {
            Room currentRoom = rooms[i, j].GetComponent<Room>();

            if(chosen == -1) {
                //Just reached level
                float dir = Random.Range(0.0f, 1.0f);

                if(dir < 0.4f) {
                    //Go left if possible
                    if(i > 0) {
                        //Left
                        chosen = 0;
                        i--;
                        currentRoom.left = true;
                        rooms[i, j].GetComponent<Room>().right = true;
                    } else {
                        //Right
                        chosen = 1;
                        i++;
                        currentRoom.right = true;
                        rooms[i, j].GetComponent<Room>().left = true;
                    }
                } else if(dir < 0.8f) {
                    //Go right if possible
                    if(i < size - 1) {
                        //Right
                        chosen = 1;
                        i++;
                        currentRoom.right = true;
                        rooms[i, j].GetComponent<Room>().left = true;
                    } else {
                        //Left
                        chosen = 0;
                        i--;
                        currentRoom.left = true;
                        rooms[i, j].GetComponent<Room>().right = true;
                    }
                } else {
                    //Go down
                    chosen = -1;
                    j++;
                    
                    if(j < size) {
                        currentRoom.down = true;
                        rooms[i, j].GetComponent<Room>().up = true;
                    }
                }
            } else {
                float dir = Random.Range(0.0f, 1.0f);

                if(dir < 0.5f) {
                    //Continue same direction
                    if(chosen == 0) {
                        //Try left
                        if(i > 0) {
                            //Left
                            chosen = 0;
                            i--;
                            currentRoom.left = true;
                            rooms[i, j].GetComponent<Room>().right = true;
                        } else {
                            //Go down
                            chosen = -1;
                            j++;

                            if(j < size) {
                                currentRoom.down = true;
                                rooms[i, j].GetComponent<Room>().up = true;
                            }
                        }
                    } else {
                        //Try right
                        if(i < size - 1) {
                            //Right
                            chosen = 1;
                            i++;
                            currentRoom.right = true;
                            rooms[i, j].GetComponent<Room>().left = true;
                        } else {
                            //Go down
                            chosen = -1;
                            j++;

                            if(j < size) {
                                currentRoom.down = true;
                                rooms[i, j].GetComponent<Room>().up = true;
                            }
                        }
                    }
                } else {
                    //Go down
                    chosen = -1;
                    j++;

                    if(j < size) {
                        currentRoom.down = true;
                        rooms[i, j].GetComponent<Room>().up = true;
                    }
                }
            }

            if(j < size) {
                rooms[i, j].GetComponent<Room>().setCritical(true);
            } else {
                rooms[i, j - 1].GetComponent<Room>().goal = true;
                rooms[i, j - 1].GetComponent<Room>().placeDiamond();
            }
        }
    }

    private void layoutRooms() {
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                if(rooms[i, j] != null) {
                    Room room = rooms[i, j].GetComponent<Room>();
                    int num = room.getAdjoining();
                    bool left = room.left;
                    bool right = room.right;
                    bool up = room.up;
                    bool down = room.down;
                    bool opposite = (left && right) || (up && down);

                    randomRoom(rooms[i, j].transform, num, opposite, rooms[i, j].GetComponent<Room>().goal);

                    switch(num) {
                        case 1:
                            if(left) {
                                room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            } else if(right) {
                                room.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                            } else if(up) {
                                room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                            } else if(down) {
                                room.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                            }

                            break;
                        case 2:
                            if(opposite) {
                                if(left)
                                    room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                                else if(up)
                                    room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                            } else {
                                if(left) {
                                    if(up)
                                        room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                                    else
                                        room.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                                } else {
                                    if(up)
                                        room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                                    else
                                        room.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                                }
                            }

                            break;
                        case 3:
                            if(!down)
                                room.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            else if(!left)
                                room.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                            else if(!up)
                                room.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                            else if(!right)
                                room.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void nonCritical() {
        bool changing = true;

        while(changing) {
            changing = false;

            for(int i = 0; i < size; i++) {
                for(int j = 0; j < size; j++) {
                    Room room = rooms[i, j].GetComponent<Room>();

                    if(room.getCritical())
                        continue;

                    if(i > 0 && !room.left) {
                        Room compareRoom = rooms[i - 1, j].GetComponent<Room>();

                        if(compareRoom.getCritical() && Random.Range(0.0f, 1.0f) < 0.2f) {
                            room.setAttached(true);
                            room.left = true;
                            compareRoom.right = true;
                            changing = true;
                        } else if(compareRoom.connectable && compareRoom.getAttached() && Random.Range(0.0f, 1.0f) < 0.35f) {
                            room.setAttached(true);
                            room.left = true;
                            compareRoom.right = true;
                            compareRoom.connectable = false;
                            changing = true;
                        }
                    }

                    if(i < size - 1 && !room.right) {
                        Room compareRoom = rooms[i + 1, j].GetComponent<Room>();

                        if(compareRoom.getCritical() && Random.Range(0.0f, 1.0f) < 0.2f) {
                            room.setAttached(true);
                            room.right = true;
                            compareRoom.left = true;
                            changing = true;
                        } else if(compareRoom.connectable && compareRoom.getAttached() && Random.Range(0.0f, 1.0f) < 0.5f) {
                            room.setAttached(true);
                            room.right = true;
                            compareRoom.left = true;
                            compareRoom.connectable = false;
                            changing = true;
                        }
                    }

                    if(j > 0 && !room.up) {
                        Room compareRoom = rooms[i, j - 1].GetComponent<Room>();

                        if(compareRoom.getCritical() && Random.Range(0.0f, 1.0f) < 0.2f) {
                            room.setAttached(true);
                            room.up = true;
                            compareRoom.down = true;
                            changing = true;
                        } else if(compareRoom.connectable && compareRoom.getAttached() && Random.Range(0.0f, 1.0f) < 0.5f) {
                            room.setAttached(true);
                            room.up = true;
                            compareRoom.down = true;
                            compareRoom.connectable = false;
                            changing = true;
                        }
                    }

                    if(j < size - 1 && !room.down) {
                        Room compareRoom = rooms[i, j + 1].GetComponent<Room>();

                        if(compareRoom.getCritical() && Random.Range(0.0f, 1.0f) < 0.2f) {
                            room.setAttached(true);
                            room.down = true;
                            compareRoom.up = true;
                            changing = true;
                        } else if(compareRoom.connectable && compareRoom.getAttached() && Random.Range(0.0f, 1.0f) < 0.5f) {
                            room.setAttached(true);
                            room.down = true;
                            compareRoom.up = true;
                            compareRoom.connectable = false;
                            changing = true;
                        }
                    }
                }
            }
        }
    }

    private void populateRooms() {
        for(int i = 0; i < size; i++) {
            for(int j = 0; j < size; j++) {
                if(rooms[i, j].GetComponent<Room>().getPopulate())
                    rooms[i, j].GetComponent<Room>().populateRoom();
            }
        }
    }

    private GameObject randomRoom(Transform parent, int num, bool opposite, bool goal) {
        float rand = Random.Range(0.0f, 1.0f);

        if(rand < 0.33f || goal) {
            parent.GetComponent<Room>().setPopulate(true);

            //Room
            switch(num) {
                case 1:
                    return Instantiate(room1, parent);
                case 2:
                    if(opposite)
                        return Instantiate(room2Straight, parent);
                    else
                        return Instantiate(room2Corner, parent);
                case 3:
                    return Instantiate(room3, parent);
                case 4:
                    return Instantiate(room4, parent);
                default:
                    return null;
            }
        } else {
            //Corridor
            switch(num) {
                case 1:
                    //No corridor with one entrance
                    return Instantiate(room1, parent);
                case 2:
                    if(opposite)
                        return Instantiate(corridorStraight, parent);
                    else
                        return Instantiate(corridorCorner, parent);
                case 3:
                    return Instantiate(corridorT, parent);
                case 4:
                    return Instantiate(corridorCross, parent);
                default:
                    return null;
            }
        }

    }
}
