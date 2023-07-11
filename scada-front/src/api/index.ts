const url = "http://localhost:7109/Api"

export const GET_TAGS = () => url + "/Tag/GetAll";
export const DELETE_TAG = () => url + "/DatabaseManager/DeleteTag";
export const UPDATE_TAG_SCAN = () => url + "/DatabaseManager/UpdateTagScan";
export const CREATE_TAG = () => url + "/DatabaseManager/CreateTag";

export const GET_ALARMS = () => url + "/Alarm/GetAll";
export const GET_ALARM = () => url + "/DatabaseManager/GetAlarm";
export const GET_ALARM_BY_TAG = () => url + "/DatabaseManager/GetAlarmByTagName";
export const CREATE_ALARM = () => url + "/DatabaseManager/CreateAlarm";
export const DELETE_ALARM = () => url + "/DatabaseManager/DeleteAlarm";

export const LOGIN = () => url + "/DatabaseManager/Login";

export const WEBSOCKET = () => "ws://localhost:7109/Api/Websocket";
