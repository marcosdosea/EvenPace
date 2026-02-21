SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- -----------------------------------------------------------------------------
-- 1. Criar banco
-- -----------------------------------------------------------------------------

CREATE DATABASE IF NOT EXISTS evenpace
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;
USE evenpace;

-- -----------------------------------------------------------------------------
-- 2. Estrutura das tabelas
-- -----------------------------------------------------------------------------

-- Administrador
DROP TABLE IF EXISTS `Administrador`;
CREATE TABLE `Administrador` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `senha` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- AvaliacaoEvento
DROP TABLE IF EXISTS `AvaliacaoEvento`;
CREATE TABLE `AvaliacaoEvento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `comentario` varchar(250) DEFAULT NULL,
  `estrela` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Corredor
DROP TABLE IF EXISTS `Corredor`;
CREATE TABLE `Corredor` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cpf` char(11) NOT NULL,
  `nome` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `dataNascimento` date NOT NULL,
  `senha` varchar(45) NOT NULL,
  `Imagem` longtext,
  PRIMARY KEY (`id`),
  UNIQUE KEY `cpf_UNIQUE` (`cpf`),
  UNIQUE KEY `email_UNIQUE1` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Organizacao (depende de Administrador)
DROP TABLE IF EXISTS `Organizacao`;
CREATE TABLE `Organizacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(70) NOT NULL,
  `cnpj` char(14) NOT NULL,
  `cpf` char(11) DEFAULT NULL,
  `telefone` varchar(14) NOT NULL,
  `cep` char(8) NOT NULL,
  `rua` varchar(45) NOT NULL,
  `bairro` varchar(45) NOT NULL,
  `cidade` varchar(45) NOT NULL,
  `numero` int NOT NULL,
  `estado` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `senha` varchar(45) NOT NULL,
  `statusSituacao` tinyint(1) NOT NULL,
  `Administrador_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `cnpj_UNIQUE` (`cnpj`),
  UNIQUE KEY `email_UNIQUE2` (`email`),
  KEY `fk_Organizacao_Admistrador1_idx` (`Administrador_id`),
  CONSTRAINT `fk_Organizacao_Admistrador1` FOREIGN KEY (`Administrador_id`) REFERENCES `Administrador` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Evento (depende de Organizacao) + coluna imagem do contexto
DROP TABLE IF EXISTS `Evento`;
CREATE TABLE `Evento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `data` datetime NOT NULL,
  `numeroParticipantes` int NOT NULL,
  `discricao` varchar(400) NOT NULL,
  `distancia3` tinyint(1) NOT NULL,
  `distancia5` tinyint(1) NOT NULL,
  `distancia7` tinyint(1) NOT NULL,
  `distancia10` tinyint(1) NOT NULL,
  `distancia15` tinyint(1) NOT NULL,
  `distancia21` tinyint(1) NOT NULL,
  `distancia42` tinyint(1) NOT NULL,
  `rua` varchar(45) NOT NULL,
  `bairro` varchar(45) NOT NULL,
  `cidade` varchar(45) NOT NULL,
  `estado` varchar(45) NOT NULL,
  `infoRetiradaKit` varchar(45) NOT NULL,
  `idOrganizacao` int unsigned NOT NULL,
  `nome` varchar(45) NOT NULL,
  `Imagem` longtext,
  PRIMARY KEY (`id`),
  KEY `fk_Evento_Organizacao_idx` (`idOrganizacao`),
  CONSTRAINT `fk_Evento_Organizacao` FOREIGN KEY (`idOrganizacao`) REFERENCES `Organizacao` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- CartaoCredito (depende de Corredor)
DROP TABLE IF EXISTS `CartaoCredito`;
CREATE TABLE `CartaoCredito` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `numero` char(16) NOT NULL,
  `dataValidade` date NOT NULL,
  `codeSeguranca` int NOT NULL,
  `nome` varchar(45) NOT NULL,
  `idCorredor` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `numero_UNIQUE` (`numero`),
  UNIQUE KEY `idCorredor_UNIQUE` (`idCorredor`),
  KEY `fk_CartaoCredito_Corredor1_idx` (`idCorredor`),
  CONSTRAINT `fk_CartaoCredito_Corredor1` FOREIGN KEY (`idCorredor`) REFERENCES `Corredor` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Cupom (depende de Evento)
DROP TABLE IF EXISTS `Cupom`;
CREATE TABLE `Cupom` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(45) NOT NULL,
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Kit (depende de Evento) + coluna Imagem
DROP TABLE IF EXISTS `Kit`;
CREATE TABLE `Kit` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `valor` decimal(10,2) NOT NULL,
  `nome` varchar(45) NOT NULL,
  `descricao` varchar(45) NOT NULL,
  `disponibilidadeP` int NOT NULL,
  `disponibilidadeG` int NOT NULL,
  `disponibilidadeM` int NOT NULL,
  `utilizadaP` tinyint NOT NULL,
  `utilizadaG` tinyint NOT NULL,
  `utilizadaM` tinyint NOT NULL,
  `idEvento` int unsigned NOT NULL,
  `statusRetiradaKit` tinyint(1) NOT NULL,
  `dataRetirada` datetime NOT NULL,
  `Imagem` longtext,
  PRIMARY KEY (`id`),
  KEY `fk_Kit_Evento1_idx` (`idEvento`),
  CONSTRAINT `fk_Kit_Evento1` FOREIGN KEY (`idEvento`) REFERENCES `Evento` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Inscricao (depende de Evento, Corredor, Kit opcional, AvaliacaoEvento opcional)
-- idKit e idAvaliacaoEvento nullable para espelhar EvenPaceContext (int?)
DROP TABLE IF EXISTS `Inscricao`;
CREATE TABLE `Inscricao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `status` varchar(45) NOT NULL,
  `dataInscricao` date NOT NULL,
  `distancia` varchar(45) NOT NULL,
  `tamanhoCamisa` varchar(45) NOT NULL,
  `tempo` time DEFAULT NULL,
  `posicao` int DEFAULT NULL,
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------------------------------
-- 3. Dados iniciais
-- -----------------------------------------------------------------------------

-- Administrador (7)
INSERT INTO `Administrador` (`nome`, `email`, `senha`) VALUES
('Admin Um', 'admin1@evenpace.com', 'senha1'),
('Admin Dois', 'admin2@evenpace.com', 'senha2'),
('Admin Tres', 'admin3@evenpace.com', 'senha3'),
('Admin Quatro', 'admin4@evenpace.com', 'senha4'),
('Admin Cinco', 'admin5@evenpace.com', 'senha5'),
('Admin Seis', 'admin6@evenpace.com', 'senha6'),
('Admin Sete', 'admin7@evenpace.com', 'senha7');

-- AvaliacaoEvento (7)
INSERT INTO `AvaliacaoEvento` (`comentario`, `estrela`) VALUES
('Ótimo evento!', 5),
('Muito bom.', 4),
('Bom organizado.', 5),
('Poderia melhorar o kit.', 3),
('Excelente.', 5),
('Legal.', 4),
('Recomendo.', 5);

-- Corredor (7)
INSERT INTO `Corredor` (`cpf`, `nome`, `email`, `dataNascimento`, `senha`, `Imagem`) VALUES
('11111111111', 'Felipe da Silva', 'c1@teste.com', '2005-01-11', 'senha1', NULL),
('22222222222', 'Luiz Eduardo', 'c2@teste.com', '1985-05-20', 'senha2', NULL),
('33333333333', 'Lorena', 'c3@teste.com', '1992-08-10', 'senha3', NULL),
('44444444444', 'Lucas', 'c4@teste.com', '1988-12-01', 'senha4', NULL),
('55555555555', 'Thiago', 'c5@teste.com', '1995-03-22', 'senha5', NULL),
('66666666666', 'Hevellny', 'c6@teste.com', '1980-07-07', 'senha6', NULL),
('77777777777', 'Alanna', 'c7@teste.com', '1998-11-30', 'senha7', NULL),
('88888888888', 'Gabriel', 'c8@teste.com', '1992-11-30', 'senha8', NULL);

-- Organizacao (7) - vincula aos administradores
INSERT INTO `Organizacao` (`nome`, `cnpj`, `cpf`, `telefone`, `cep`, `rua`, `bairro`, `cidade`, `numero`, `estado`, `email`, `senha`, `statusSituacao`, `Administrador_id`) VALUES
('Velocity Vortex Championship', '11111111000191', '11111111111', '79999990001', '49000001', 'Rua A', 'Centro', 'Aracaju', 100, 'SE', 'org1@teste.com', 'senha', 1, 1),
('Nitro Knights Racing League', '22222222000192', '22222222222', '79999990002', '49000002', 'Rua B', 'Atalaia', 'Aracaju', 200, 'SE', 'org2@teste.com', 'senha', 1, 2),
('Apex Accelerators', '33333333000193', '33333333333', '79999990003', '49000003', 'Rua C', 'Jardins', 'São Paulo', 300, 'SP', 'org3@teste.com', 'senha', 1, 3),
('Circuit Crew Syndicate', '44444444000194', '44444444444', '79999990004', '49000004', 'Rua D', 'Barra', 'Salvador', 400, 'BA', 'org4@teste.com', 'senha', 1, 4),
('Burnout Brigade', '55555555000195', '55555555555', '79999990005', '49000005', 'Rua E', 'Copacabana', 'Rio de Janeiro', 500, 'RJ', 'org5@teste.com', 'senha', 1, 5),
('Drift Dynamics Pro', '66666666000196', '66666666666', '79999990006', '49000006', 'Rua F', 'Ponta Verde', 'Maceió', 600, 'AL', 'org6@teste.com', 'senha', 1, 6),
('Thunder Torque Series', '77777777000197', '77777777777', '79999990007', '49000007', 'Rua G', 'Boa Viagem', 'Recife', 700, 'PE', 'org7@teste.com', 'senha', 1, 7);

-- Evento (7) - um por organização, datas variadas
INSERT INTO `Evento` (`data`, `numeroParticipantes`, `discricao`, `distancia3`, `distancia5`, `distancia7`, `distancia10`, `distancia15`, `distancia21`, `distancia42`, `rua`, `bairro`, `cidade`, `estado`, `infoRetiradaKit`, `idOrganizacao`, `nome`, `Imagem`) VALUES
('2025-06-01 08:00:00', 500, 'Corrida de rua 5k e 10k', 1, 1, 0, 1, 0, 0, 0, 'Av. Beira Mar', 'Atalaia', 'Aracaju', 'SE', 'Secretaria no dia', 1, 'Corrida Beira Mar', NULL),
('2025-07-15 07:00:00', 300, 'Meia maratona e 5k', 0, 1, 0, 0, 1, 1, 0, 'Parque Ibirapuera', 'Moema', 'São Paulo', 'SP', 'Tenda principal', 2, 'Corrida Ibirapuera', NULL),
('2025-08-20 06:30:00', 1000, 'Maratona completa e meia', 0, 0, 0, 0, 0, 1, 1, 'Orla', 'Barra', 'Salvador', 'BA', 'Retirada no evento', 3, 'Maratona Salvador', NULL),
('2025-09-10 08:00:00', 200, 'Corrida 3k e 5k infantil', 1, 1, 0, 0, 0, 0, 0, 'Praia', 'Copacabana', 'Rio de Janeiro', 'RJ', 'Local do evento', 4, 'Corrida Kids RJ', NULL),
('2025-10-05 07:00:00', 400, 'Corrida 5k e 10k', 0, 1, 0, 1, 0, 0, 0, 'Av. Beira Mar', 'Ponta Verde', 'Maceió', 'AL', 'Dia do evento', 5, 'Corrida Maceió', NULL),
('2025-11-12 06:00:00', 600, 'Meia maratona 21k', 0, 0, 0, 0, 0, 1, 0, 'Orla', 'Boa Viagem', 'Recife', 'PE', 'Tenda de retirada', 6, 'Meia Maratona Recife', NULL),
('2025-12-25 07:30:00', 350, 'Corrida de Natal 5k/10k', 0, 1, 0, 1, 0, 0, 0, 'Centro', 'Centro', 'Aracaju', 'SE', 'Secretaria', 7, 'Corrida de Natal', NULL);

-- CartaoCredito (5 - 1 por corredor até 5, único por corredor)
INSERT INTO `CartaoCredito` (`numero`, `dataValidade`, `codeSeguranca`, `nome`, `idCorredor`) VALUES
('4111111111111111', '2027-12-01', 123, 'Nubank', 1),
('4222222222222222', '2028-06-01', 456, 'Santander', 2),
('4333333333333333', '2027-09-01', 789, 'Will Bank', 3),
('4444444444444444', '2028-03-01', 321, 'Santander', 4),
('4555555555555555', '2027-11-01', 654, 'Nubank', 5),
('4233333333333333', '2027-11-01', 653, 'Nubank', 7),
('4233333333222223', '2028-12-01', 631, 'Will Bank', 8);

-- Cupom (7 - um por evento)
INSERT INTO `Cupom` (`nome`, `desconto`, `status`, `dataInicio`, `dataTermino`, `quantidadeUtilizada`, `quantiadeDisponibilizada`, `idEvento`) VALUES
('PROMO5', 5, 1, '2025-01-01', '2025-05-31', 0, 100, 1),
('VERAO10', 10, 1, '2025-06-01', '2025-07-14', 0, 50, 2),
('MEIA20', 20, 1, '2025-07-01', '2025-08-19', 0, 30, 3),
('KIDS15', 15, 1, '2025-08-01', '2025-09-09', 0, 80, 4),
('MACEIO10', 10, 1, '2025-09-01', '2025-10-04', 0, 60, 5),
('RECIFE25', 25, 1, '2025-10-01', '2025-11-11', 0, 40, 6),
('NATAL30', 30, 1, '2025-11-01', '2025-12-24', 0, 70, 7);

-- Kit (7 - um por evento)
INSERT INTO `Kit` (`valor`, `nome`, `descricao`, `disponibilidadeP`, `disponibilidadeG`, `disponibilidadeM`, `utilizadaP`, `utilizadaG`, `utilizadaM`, `idEvento`, `statusRetiradaKit`, `dataRetirada`, `Imagem`) VALUES
(49.90, 'Kit Básico Beira Mar', 'Camiseta', 50, 50, 50, 0, 0, 0, 1, 0, '2025-05-31 14:00:00', NULL),
(79.90, 'Kit Ibirapuera', 'Camiseta + Medalha', 30, 30, 30, 0, 0, 0, 2, 0, '2025-07-14 14:00:00', NULL),
(129.90, 'Kit Maratona', 'Camiseta + Medalha + Mochila', 20, 20, 20, 0, 0, 0, 3, 0, '2025-08-19 14:00:00', NULL),
(39.90, 'Kit Kids', 'Camiseta infantil', 40, 40, 40, 0, 0, 0, 4, 0, '2025-09-09 14:00:00', NULL),
(59.90, 'Kit Maceió', 'Camiseta + Boné', 35, 35, 35, 0, 0, 0, 5, 0, '2025-10-04 14:00:00', NULL),
(89.90, 'Kit Meia Recife', 'Camiseta + Medalha', 25, 25, 25, 0, 0, 0, 6, 0, '2025-11-11 14:00:00', NULL),
(69.90, 'Kit Natal', 'Camiseta temática', 45, 45, 45, 0, 0, 0, 7, 0, '2025-12-24 14:00:00', NULL);

-- Inscricao (7 - evento, corredor, kit e avaliacao opcionais)
INSERT INTO `Inscricao` (`status`, `dataInscricao`, `distancia`, `tamanhoCamisa`, `tempo`, `posicao`, `idKit`, `idEvento`, `idCorredor`, `idAvaliacaoEvento`) VALUES
('Confirmada', '2025-01-10', '5km', 'M', NULL, 0, 1, 1, 1, 1),
('Pendente', '2025-02-01', '10km', 'G', NULL, 0, 1, 1, 2, NULL),
('Confirmada', '2025-02-15', '5km', 'P', NULL, 0, 2, 2, 2, 2),
('Cancelada', '2025-01-20', '5km', 'M', NULL, 0, 1, 1, 3, NULL),
('Confirmada', '2025-03-01', '21km', 'G', NULL, 0, 3, 3, 3, 3),
('Pendente', '2025-03-10', '5km', 'M', NULL, 0, 4, 4, 4, NULL),
('Confirmada', '2025-04-01', '10km', 'M', NULL, 0, 5, 5, 5, 4);

SET FOREIGN_KEY_CHECKS = 1;