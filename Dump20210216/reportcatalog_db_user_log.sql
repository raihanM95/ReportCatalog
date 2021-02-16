-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: reportcatalog_db
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `user_log`
--

DROP TABLE IF EXISTS `user_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_log` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `userType` varchar(45) NOT NULL,
  `time` datetime NOT NULL,
  `type` varchar(45) NOT NULL,
  `ip` varchar(55) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_log`
--

LOCK TABLES `user_log` WRITE;
/*!40000 ALTER TABLE `user_log` DISABLE KEYS */;
INSERT INTO `user_log` VALUES (14,'raihan','user','2021-02-16 10:15:23','Login','192.168.1.36'),(15,'raihan','user','2021-02-16 10:15:36','Logout','192.168.1.36'),(16,'admin','admin','2021-02-16 10:16:00','Login','192.168.1.36'),(17,'admin','admin','2021-02-16 10:21:22','Logout','192.168.1.36'),(18,'raihan','user','2021-02-16 10:21:35','Login','192.168.1.36'),(19,'raihan','user','2021-02-16 10:22:15','Logout','192.168.1.36'),(22,'raihan','user','2021-02-16 10:32:52','Login','192.168.1.36'),(23,'raihan','user','2021-02-16 10:38:16','Logout','192.168.1.36'),(24,'raihan','user','2021-02-16 10:44:22','Login','192.168.1.36'),(25,'raihan','user','2021-02-16 10:47:48','Logout','192.168.1.36'),(26,'admin','admin','2021-02-16 10:48:07','Login','192.168.1.36'),(27,'tusar','user','2021-02-16 11:17:56','Login','192.168.1.68'),(28,'admin','admin','2021-02-16 11:18:26','Login','192.168.1.36'),(29,'admin','admin','2021-02-16 11:36:38','Login','192.168.1.36'),(30,'admin','admin','2021-02-16 11:37:37','Login','192.168.1.36'),(31,'admin','admin','2021-02-16 11:37:58','Logout','192.168.1.36'),(32,'sojib','user','2021-02-16 11:38:54','Login','192.168.1.223'),(33,'sojib','user','2021-02-16 11:40:04','Logout','192.168.1.223'),(34,'admin','admin','2021-02-16 12:38:02','Login','192.168.1.36'),(35,'admin','admin','2021-02-16 12:43:56','Logout','192.168.1.36');
/*!40000 ALTER TABLE `user_log` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-02-16 12:47:26
