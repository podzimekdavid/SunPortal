# SunPortal

SunPortal is a conceptual microservices-based system designed for monitoring and controlling solar power installations. The system architecture facilitates communication between services using RabbitMQ and Http protocols.

For interaction between the server and client devices, SignalR or HTTP requests are used. The client devices are based on the [UniPi Patron S107-P370](https://www.unipi.technology/cs/unipi-patron-s107-p370), an industrial-grade PLC (Programmable Logic Controller) platform.

SunPortal has been tested with solar inverters and MPPT controllers from Studer, with data acquisition performed through the appropriate protocols.

The entire project was developed in about one week. While it is not perfect or production-ready, I really enjoyed working on it.

---

## Key Features

- Modular microservices architecture for scalability and maintainability  
- Communication via RabbitMQ messaging brokers 
- Real-time server-client communication using SignalR and HTTP requests  
- Integration with industrial PLC hardware (UniPi Patron)  
- Support for solar inverter and MPPT data collection  
- Dashboard and graph creation for solar system monitoring and visualization

---

## Hardware

- [UniPi Patron S107-P370](https://www.unipi.technology/cs/unipi-patron-s107-p370) — industrial PLC acting as a client device  
- Studer devices (inverters, MPPT solar controllers)

---

