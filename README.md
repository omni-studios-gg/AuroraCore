# AuroraCore  
**A Modular MMORPG Server Framework**  

![AuroraCore Banner](https://via.placeholder.com/1200x400?text=AuroraCore+Architecture)  
*(Replace with actual banner image)*  

---

## 🚀 Features  
- **Secure Authentication**: Token-based auth with IP tracking and brute-force protection  
- **Dynamic World Servers**: Unity-based sharding with Zone/Player management  
- **Real-time Chat**: Redis-backed pub/sub for cross-server communication  
- **Moderation Tools**: Activity logging with Discord webhook alerts  
- **Planned**: Cross-realm inventory and auction system (Q4 2024)  

---


## 🛠️ Tech Stack
Service	Technologies
AuthServer	ASP.NET Core, SQLite/MySQL
WorldServer	Unity 2022+, Mirror Networking, C#
Chat Service	FastAPI/Node.js, Redis, WebSocket
Logging	PostgreSQL, Discord Webhooks

## 📂 Project Structure  
```plaintext
AuroraCore/
├── Auth/                # AuthServer + Logging (ASP.NET Core)
├── World/               # WorldServer + Managers (Unity)
├── Chat/                # Chat Service (FastAPI/Node.js)
├── Inventory/           # (Planned) Item/auction system
├── Libs/                # Shared core libraries
└── Tools/               # Developer utilities```


