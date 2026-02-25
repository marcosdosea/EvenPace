-- MySQL dump 10.13  Distrib 9.5.0, for Linux (x86_64)
--
-- Host: localhost    Database: evenpace
-- ------------------------------------------------------
-- Server version	9.5.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'e7076c7f-eb34-11f0-9650-3c219c9746f4:1-536';

--
-- Table structure for table `Administrador`
--

DROP TABLE IF EXISTS `Administrador`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Administrador` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Administrador`
--

LOCK TABLES `Administrador` WRITE;
/*!40000 ALTER TABLE `Administrador` DISABLE KEYS */;
INSERT INTO `Administrador` VALUES (1,'Admin Um'),(2,'Admin Dois'),(3,'Admin Tres'),(4,'Admin Quatro'),(5,'Admin Cinco'),(6,'Admin Seis'),(7,'Admin Sete');
/*!40000 ALTER TABLE `Administrador` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AvaliacaoEvento`
--

DROP TABLE IF EXISTS `AvaliacaoEvento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AvaliacaoEvento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `comentario` varchar(250) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `estrela` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AvaliacaoEvento`
--

LOCK TABLES `AvaliacaoEvento` WRITE;
/*!40000 ALTER TABLE `AvaliacaoEvento` DISABLE KEYS */;
INSERT INTO `AvaliacaoEvento` VALUES (1,'Ótimo evento!',5),(2,'Muito bom.',4),(3,'Bom organizado.',5),(4,'Poderia melhorar o kit.',3),(5,'Excelente.',5),(6,'Legal.',4),(7,'Recomendo.',5);
/*!40000 ALTER TABLE `AvaliacaoEvento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CartaoCredito`
--

DROP TABLE IF EXISTS `CartaoCredito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CartaoCredito` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `numero` char(16) COLLATE utf8mb4_unicode_ci NOT NULL,
  `dataValidade` date NOT NULL,
  `codeSeguranca` int NOT NULL,
  `nome` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idCorredor` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `numero_UNIQUE` (`numero`),
  UNIQUE KEY `idCorredor_UNIQUE` (`idCorredor`),
  KEY `fk_CartaoCredito_Corredor1_idx` (`idCorredor`),
  CONSTRAINT `fk_CartaoCredito_Corredor1` FOREIGN KEY (`idCorredor`) REFERENCES `Corredor` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CartaoCredito`
--

LOCK TABLES `CartaoCredito` WRITE;
/*!40000 ALTER TABLE `CartaoCredito` DISABLE KEYS */;
INSERT INTO `CartaoCredito` VALUES (1,'4111111111111111','2027-12-01',123,'Nubank',1),(2,'4222222222222222','2028-06-01',456,'Santander',2),(3,'4333333333333333','2027-09-01',789,'Will Bank',3),(4,'4444444444444444','2028-03-01',321,'Santander',4),(5,'4555555555555555','2027-11-01',654,'Nubank',5),(6,'4233333333333333','2027-11-01',653,'Nubank',7),(7,'4233333333222223','2028-12-01',631,'Will Bank',8);
/*!40000 ALTER TABLE `CartaoCredito` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Corredor`
--

DROP TABLE IF EXISTS `Corredor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Corredor` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cpf` char(11) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nome` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `dataNascimento` date NOT NULL,
  `Imagem` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`id`),
  UNIQUE KEY `cpf_UNIQUE` (`cpf`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Corredor`
--

LOCK TABLES `Corredor` WRITE;
/*!40000 ALTER TABLE `Corredor` DISABLE KEYS */;
INSERT INTO `Corredor` VALUES (1,'11111111111','Felipe da Silva','2005-01-11',NULL),(2,'22222222222','Luiz Eduardo','1985-05-20',NULL),(3,'33333333333','Lorena','1992-08-10',NULL),(4,'44444444444','Lucas','1988-12-01',NULL),(5,'55555555555','Thiago','1995-03-22',NULL),(6,'66666666666','Hevellny','1980-07-07',NULL),(7,'77777777777','Alanna','1998-11-30',NULL),(8,'88888888888','Gabriel','1992-11-30',NULL),(9,'12345678991','Lucas','1231-03-12',NULL);
/*!40000 ALTER TABLE `Corredor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Cupom`
--

DROP TABLE IF EXISTS `Cupom`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Cupom` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `desconto` int NOT NULL,
  `status` tinyint(1) NOT NULL,
  `dataInicio` date NOT NULL,
  `dataTermino` date NOT NULL,
  `quantidadeUtilizada` int NOT NULL,
  `quantiadeDisponibilizada` int NOT NULL,
  `idEvento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Cupom_Evento1_idx` (`idEvento`),
  CONSTRAINT `fk_Cupom_Evento1` FOREIGN KEY (`idEvento`) REFERENCES `Evento` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Cupom`
--

LOCK TABLES `Cupom` WRITE;
/*!40000 ALTER TABLE `Cupom` DISABLE KEYS */;
INSERT INTO `Cupom` VALUES (1,'PROMO5',5,1,'2025-01-01','2025-05-31',0,100,1),(2,'VERAO10',10,1,'2025-06-01','2025-07-14',0,50,2),(3,'MEIA20',20,1,'2025-07-01','2025-08-19',0,30,3),(4,'KIDS15',15,1,'2025-08-01','2025-09-09',0,80,4),(5,'MACEIO10',10,1,'2025-09-01','2025-10-04',0,60,5),(6,'RECIFE25',25,1,'2025-10-01','2025-11-11',0,40,6),(7,'NATAL30',30,1,'2025-11-01','2025-12-24',0,70,7);
/*!40000 ALTER TABLE `Cupom` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Evento`
--

DROP TABLE IF EXISTS `Evento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Evento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `data` datetime NOT NULL,
  `numeroParticipantes` int NOT NULL,
  `discricao` varchar(400) COLLATE utf8mb4_unicode_ci NOT NULL,
  `distancia3` tinyint(1) NOT NULL,
  `distancia5` tinyint(1) NOT NULL,
  `distancia7` tinyint(1) NOT NULL,
  `distancia10` tinyint(1) NOT NULL,
  `distancia15` tinyint(1) NOT NULL,
  `distancia21` tinyint(1) NOT NULL,
  `distancia42` tinyint(1) NOT NULL,
  `rua` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `bairro` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cidade` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `estado` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `infoRetiradaKit` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idOrganizacao` int unsigned NOT NULL,
  `nome` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Imagem` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`id`),
  KEY `fk_Evento_Organizacao_idx` (`idOrganizacao`),
  CONSTRAINT `fk_Evento_Organizacao` FOREIGN KEY (`idOrganizacao`) REFERENCES `Organizacao` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Evento`
--

LOCK TABLES `Evento` WRITE;
/*!40000 ALTER TABLE `Evento` DISABLE KEYS */;
INSERT INTO `Evento` VALUES (1,'2025-06-01 08:00:00',500,'Corrida de rua 5k e 10k',1,1,0,1,0,0,0,'Av. Beira Mar','Atalaia','Aracaju','SE','Secretaria no dia',1,'Corrida Beira Mar','evento.png'),(2,'2025-07-15 07:00:00',300,'Meia maratona e 5k',0,1,0,0,1,1,0,'Parque Ibirapuera','Moema','São Paulo','SP','Tenda principal',2,'Corrida Ibirapuera','evento2.png'),(3,'2025-08-20 06:30:00',1000,'Maratona completa e meia',0,0,0,0,0,1,1,'Orla','Barra','Salvador','BA','Retirada no evento',3,'Maratona Salvador','evento3.png'),(4,'2025-09-10 08:00:00',200,'Corrida 3k e 5k infantil',1,1,0,0,0,0,0,'Praia','Copacabana','Rio de Janeiro','RJ','Local do evento',4,'Corrida Kids RJ','evento4.png'),(5,'2025-10-05 07:00:00',400,'Corrida 5k e 10k',0,1,0,1,0,0,0,'Av. Beira Mar','Ponta Verde','Maceió','AL','Dia do evento',5,'Corrida Maceió','evento.png'),(6,'2025-11-12 06:00:00',600,'Meia maratona 21k',0,0,0,0,0,1,0,'Orla','Boa Viagem','Recife','PE','Tenda de retirada',6,'Meia Maratona Recife','evento2.png'),(7,'2025-12-25 07:30:00',350,'Corrida de Natal 5k/10k',0,1,0,1,0,0,0,'Centro','Centro','Aracaju','SE','Secretaria',7,'Corrida de Natal','evento3.png');
/*!40000 ALTER TABLE `Evento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Inscricao`
--

DROP TABLE IF EXISTS `Inscricao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Inscricao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `status` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `dataInscricao` date NOT NULL,
  `distancia` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `tamanhoCamisa` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `tempo` time DEFAULT NULL,
  `posicao` int DEFAULT NULL,
  `statusRetiradaKit` tinyint(1) NOT NULL,
  `idKit` int unsigned DEFAULT NULL,
  `idEvento` int unsigned NOT NULL,
  `idCorredor` int unsigned NOT NULL,
  `idAvaliacaoEvento` int unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Inscricao_AvaliacaoEvento1_idx` (`idAvaliacaoEvento`),
  KEY `fk_Inscricao_Corredor1_idx` (`idCorredor`),
  KEY `fk_Inscricao_Evento1_idx` (`idEvento`),
  KEY `fk_Inscricao_Kit1_idx` (`idKit`),
  CONSTRAINT `fk_Inscricao_AvaliacaoEvento1` FOREIGN KEY (`idAvaliacaoEvento`) REFERENCES `AvaliacaoEvento` (`id`),
  CONSTRAINT `fk_Inscricao_Corredor1` FOREIGN KEY (`idCorredor`) REFERENCES `Corredor` (`id`),
  CONSTRAINT `fk_Inscricao_Evento1` FOREIGN KEY (`idEvento`) REFERENCES `Evento` (`id`),
  CONSTRAINT `fk_Inscricao_Kit1` FOREIGN KEY (`idKit`) REFERENCES `Kit` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Inscricao`
--

LOCK TABLES `Inscricao` WRITE;
/*!40000 ALTER TABLE `Inscricao` DISABLE KEYS */;
INSERT INTO `Inscricao` VALUES (1,'Confirmada','2025-01-10','5km','M',NULL,0,0,1,1,1,1),(2,'Pendente','2025-02-01','10km','G',NULL,0,0,1,1,2,NULL),(3,'Confirmada','2025-02-15','5km','P',NULL,0,0,2,2,2,2),(4,'Cancelada','2025-01-20','5km','M',NULL,0,0,1,1,3,NULL),(5,'Confirmada','2025-03-01','21km','G',NULL,0,0,3,3,3,3),(6,'Pendente','2025-03-10','5km','M',NULL,0,0,4,4,4,NULL),(7,'Confirmada','2025-04-01','10km','M',NULL,0,0,5,5,5,4),(8,'Confirmada','2026-02-24','5km','M',NULL,NULL,0,1,1,1,NULL),(9,'Confirmada','2026-02-24','5km','M',NULL,NULL,0,1,1,1,NULL),(10,'Confirmada','2026-02-24','5km','M',NULL,NULL,0,1,7,1,NULL),(15,'Pendente','2026-02-24','10km','P',NULL,NULL,0,3,3,1,NULL),(16,'Pendente','2026-02-24','10km','P',NULL,NULL,0,3,3,9,NULL),(17,'Pendente','2026-02-24','10km','P',NULL,NULL,0,4,4,9,NULL),(18,'Pendente','2026-02-24','5km','P',NULL,NULL,0,5,5,9,NULL),(19,'Pendente','2026-02-24','10km','GG',NULL,NULL,0,2,2,9,NULL),(20,'Pendente','2026-02-24','5km','P',NULL,NULL,0,3,3,9,NULL),(21,'Pendente','2026-02-24','5km','P',NULL,NULL,0,3,3,9,NULL),(22,'Pendente','2026-02-24','5km','G',NULL,NULL,0,1,1,9,NULL),(23,'Pendente','2026-02-24','5km','G',NULL,NULL,0,1,1,9,NULL),(24,'Pendente','2026-02-24','3km','GG',NULL,NULL,0,1,1,9,NULL),(25,'Pendente','2026-02-24','5km','G',NULL,NULL,0,2,2,9,NULL);
/*!40000 ALTER TABLE `Inscricao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Kit`
--

DROP TABLE IF EXISTS `Kit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Kit` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `valor` decimal(10,2) NOT NULL,
  `nome` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descricao` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `disponibilidadeP` int NOT NULL,
  `disponibilidadeG` int NOT NULL,
  `disponibilidadeM` int NOT NULL,
  `utilizadaP` tinyint NOT NULL,
  `utilizadaG` tinyint NOT NULL,
  `utilizadaM` tinyint NOT NULL,
  `idEvento` int unsigned NOT NULL,
  `dataRetirada` datetime NOT NULL,
  `Imagem` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`id`),
  KEY `fk_Kit_Evento1_idx` (`idEvento`),
  CONSTRAINT `fk_Kit_Evento1` FOREIGN KEY (`idEvento`) REFERENCES `Evento` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Kit`
--

LOCK TABLES `Kit` WRITE;
/*!40000 ALTER TABLE `Kit` DISABLE KEYS */;
INSERT INTO `Kit` VALUES (1,49.90,'Kit Básico Beira Mar','Camiseta',50,50,50,0,0,0,1,'2025-05-31 14:00:00','kit1.png'),(2,79.90,'Kit Ibirapuera','Camiseta + Medalha',30,30,30,0,0,0,2,'2025-07-14 14:00:00','kit2.png'),(3,129.90,'Kit Maratona','Camiseta + Medalha + Mochila',20,20,20,0,0,0,3,'2025-08-19 14:00:00','kit1.png'),(4,39.90,'Kit Kids','Camiseta infantil',40,40,40,0,0,0,4,'2025-09-09 14:00:00','kit2.png'),(5,59.90,'Kit Maceió','Camiseta + Boné',35,35,35,0,0,0,5,'2025-10-04 14:00:00','kit1.png'),(6,89.90,'Kit Meia Recife','Camiseta + Medalha',25,25,25,0,0,0,6,'2025-11-11 14:00:00',NULL),(7,69.90,'Kit Natal','Camiseta temática',45,45,45,0,0,0,7,'2025-12-24 14:00:00','kit1.png');
/*!40000 ALTER TABLE `Kit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Organizacao`
--

DROP TABLE IF EXISTS `Organizacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Organizacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(70) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cnpj` char(14) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cpf` char(11) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `telefone` varchar(14) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cep` char(8) COLLATE utf8mb4_unicode_ci NOT NULL,
  `rua` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `bairro` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cidade` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `numero` int NOT NULL,
  `estado` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `statusSituacao` tinyint(1) NOT NULL,
  `Administrador_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `cnpj_UNIQUE` (`cnpj`),
  KEY `fk_Organizacao_Admistrador1_idx` (`Administrador_id`),
  CONSTRAINT `fk_Organizacao_Admistrador1` FOREIGN KEY (`Administrador_id`) REFERENCES `Administrador` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Organizacao`
--

LOCK TABLES `Organizacao` WRITE;
/*!40000 ALTER TABLE `Organizacao` DISABLE KEYS */;
INSERT INTO `Organizacao` VALUES (1,'Velocity Vortex Championship','11111111000191','11111111111','79999990001','49000001','Rua A','Centro','Aracaju',100,'SE',1,1),(2,'Nitro Knights Racing League','22222222000192','22222222222','79999990002','49000002','Rua B','Atalaia','Aracaju',200,'SE',1,2),(3,'Apex Accelerators','33333333000193','33333333333','79999990003','49000003','Rua C','Jardins','São Paulo',300,'SP',1,3),(4,'Circuit Crew Syndicate','44444444000194','44444444444','79999990004','49000004','Rua D','Barra','Salvador',400,'BA',1,4),(5,'Burnout Brigade','55555555000195','55555555555','79999990005','49000005','Rua E','Copacabana','Rio de Janeiro',500,'RJ',1,5),(6,'Drift Dynamics Pro','66666666000196','66666666666','79999990006','49000006','Rua F','Ponta Verde','Maceió',600,'AL',1,6),(7,'Thunder Torque Series','77777777000197','77777777777','79999990007','49000007','Rua G','Boa Viagem','Recife',700,'PE',1,7);
/*!40000 ALTER TABLE `Organizacao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'evenpace'
--
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-02-24 20:55:22
