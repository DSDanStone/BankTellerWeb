-- Switch to the system (aka master) database
USE master;
GO

-- Delete the reviewdb Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='reviewdb')
DROP DATABASE BankDB;
GO

-- Create a new reviewdb Database
CREATE DATABASE BankDB;
GO

-- Switch to the reviewdb Database
USE BankDB
GO

drop table customers;

create table customers
(
	customer_id int identity(1,1),
	first_name varchar(200) not null,
	last_name varchar(200) not null,
	username varchar(100) unique not null,	
	password varchar(100) not null,
	address1 varchar(200) not null,
	address2 varchar(200),
	city varchar(50) not null,
	state varchar(13) not null,
	zipcode varchar(9) not null,

	constraint pk_customers primary key (customer_id)
)

drop table accounts;

create table accounts
(
	account_id int identity(1,1),
	account_name varchar(100),
	account_type varchar(50) not null,
	account_number varchar(50) unique not null,
	balance money default 0,

	constraint pk_accounts primary key (account_id)
);

drop table customer_account;

create table customer_account
(	
	customer_id int not null,
	account_id int not null,
		
	constraint pk_customer_account primary key (customer_id,account_id),
	constraint fk_customer_account_customers foreign key (customer_id) references customers(customer_id),
	constraint fk_customer_account_accounts foreign key (account_id) references accounts(account_id)
);

drop table transaction_log;

create table transaction_log
(	
	log_id int identity(1,1),
	customer_id int not null,
	account_id int not null,
	transaction_type varchar(50) not null,
	ammount money default 0,	
	log_date date default getdate(),
		
	constraint pk_transaction_log primary key (log_id),
	constraint fk_transaction_log_customers foreign key (customer_id) references customers(customer_id),
	constraint fk_transaction_log_accounts foreign key (account_id) references accounts(account_id)
);

--Add sample data
insert into customers (first_name, last_name, username, password, address1, address2, city, state, zipcode) values ('Dan', 'Stone', 'DanTheMan', 'password', '123 Main st.', ' ', 'Gburg', 'MD', '20877');


select * from customer_account;

select * from accounts;