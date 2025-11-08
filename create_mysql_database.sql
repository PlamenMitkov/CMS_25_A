-- ========================================
-- MySQL/MariaDB Database Creation Script
-- CMS_25_A (Car Management System)
-- ========================================
-- Execute this script in HeidiSQL to create your database
-- ========================================

-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS CMS_25_A 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_general_ci;

-- Use the database
USE CMS_25_A;

-- ========================================
-- Table: tblCustomer
-- ========================================
CREATE TABLE IF NOT EXISTS tblCustomer (
    CarNo VARCHAR(15) NOT NULL,
    Name VARCHAR(50) NULL,
    Address VARCHAR(50) NULL,
    Model VARCHAR(50) NULL,
    PRIMARY KEY (CarNo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ========================================
-- Table: tblWorker
-- ========================================
CREATE TABLE IF NOT EXISTS tblWorker (
    WorkerID INT NOT NULL,
    Name VARCHAR(50) NULL,
    PRIMARY KEY (WorkerID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ========================================
-- Table: tblJobDetails
-- ========================================
CREATE TABLE IF NOT EXISTS tblJobDetails (
    JobID INT NOT NULL AUTO_INCREMENT,
    CarNo VARCHAR(50) NOT NULL,
    JobDate DATETIME NOT NULL,
    WorkerID INT NOT NULL,
    KMs VARCHAR(10) NULL,
    Tuning VARCHAR(10) NULL,
    Alignment VARCHAR(10) NULL,
    Balancing VARCHAR(10) NULL,
    Tires VARCHAR(10) NULL,
    Weight VARCHAR(10) NULL,
    OilChanged VARCHAR(10) NULL,
    OilQty VARCHAR(10) NULL,
    OilFilter VARCHAR(10) NULL,
    GearOil VARCHAR(10) NULL,
    GearOilQty VARCHAR(10) NULL,
    Point VARCHAR(10) NULL,
    Condenser VARCHAR(10) NULL,
    Plug VARCHAR(10) NULL,
    PlugQty VARCHAR(10) NULL,
    FuelFilter VARCHAR(10) NULL,
    AirFilter VARCHAR(10) NULL,
    Remarks VARCHAR(10) NULL,
    PRIMARY KEY (JobID),
    FOREIGN KEY (CarNo) REFERENCES tblCustomer(CarNo) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (WorkerID) REFERENCES tblWorker(WorkerID) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ========================================
-- Sample Data (Optional - Remove if not needed)
-- ========================================
-- INSERT INTO tblWorker (WorkerID, Name) VALUES 
-- (1, 'John Smith'),
-- (2, 'Jane Doe');

-- INSERT INTO tblCustomer (CarNo, Name, Address, Model) VALUES 
-- ('ABC123', 'Customer One', '123 Main St', 'Toyota Camry'),
-- ('XYZ789', 'Customer Two', '456 Oak Ave', 'Honda Accord');

-- ========================================
-- Verify Tables Created
-- ========================================
SHOW TABLES;

-- View table structures
DESCRIBE tblCustomer;
DESCRIBE tblWorker;
DESCRIBE tblJobDetails;
