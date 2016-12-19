-- ---
-- Globals
-- ---

-- SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
-- SET FOREIGN_KEY_CHECKS=0;

-- ---
-- Table beers
--
-- ---

DROP TABLE IF EXISTS beers;

CREATE TABLE beers (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255),
  abv DECIMAL,
  ibu DECIMAL,
);

-- ---
-- Table users
--
-- ---

DROP TABLE IF EXISTS users;

CREATE TABLE users (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(25),
);

-- ---
-- Table favorites
--
-- ---

DROP TABLE IF EXISTS favorites;

CREATE TABLE favorites (
  id INT IDENTITY(1,1) PRIMARY KEY,
  beer_id INT,
  user_id INT,
  rating INT,
);

-- ---
-- Table breweries
--
-- ---

DROP TABLE IF EXISTS breweries;

CREATE TABLE breweries (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  location VARCHAR(255),
);

-- ---
-- Table beers_breweries
--
-- ---

DROP TABLE IF EXISTS beers_breweries;

CREATE TABLE beers_breweries (
  id INT IDENTITY(1,1) PRIMARY KEY,
  beer_id INT,
  brewery_id INT,
);

-- ---
-- Table beer_styles
--
-- ---

DROP TABLE IF EXISTS beer_styles;

CREATE TABLE beer_styles (
  id INT IDENTITY(1,1) PRIMARY KEY,
  beer_id INT,
  style VARCHAR(25),
);
