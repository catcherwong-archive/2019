package db

import (
	"database/sql"
	"fmt"
	"time"

	_ "github.com/lib/pq"
)

type PgDemo struct {
	Host   string
	Pwd    string
	Port   int
	DbName string
	User   string
}

func ShowPgDemo() {

	p := PgDemo{Host: "localhost", Port: 5432, Pwd: "123456", User: "postgres", DbName: "demo"}

	p.PingPg()

	p.queryRow()

	p.queryAll()

	p.insert()

	p.update()

	p.queryAll()

	p.delete()

	p.queryAll()
}

func (r PgDemo) connDb() *sql.DB {
	connStr := fmt.Sprintf("host=%s port=%d user=%s password=%s dbname=%s sslmode=%s", r.Host, r.Port, r.User, r.Pwd, r.DbName, "disable")
	db, err := sql.Open("postgres", connStr)
	if err != nil {
		panic(err)
	}

	err = db.Ping()
	if err != nil {
		panic(err)
	}

	return db
}

func (r PgDemo) PingPg() {

	_ = r.connDb()

	fmt.Println("Successfully connected!")
}

func (r PgDemo) queryRow() {

	db := r.connDb()

	s := Student{}

	sql := "select id, name, gender, create_time from t1 order by create_time desc limit 1"

	fmt.Println(db.Ping())

	err := db.QueryRow(sql).Scan(&s.id, &s.name, &s.gender, &s.create_time)

	if err != nil {
		panic(err)
	}

	fmt.Println(s)
}

func (r PgDemo) queryAll() {

	db := r.connDb()

	sql := "select id, name, gender, create_time from t1"

	fmt.Println(db.Ping())

	rows, err := db.Query(sql)

	if err != nil {
		panic(err)
	}

	defer rows.Close()

	arr := []Student{}

	for rows.Next() {

		s := Student{}

		err = rows.Scan(&s.id, &s.name, &s.gender, &s.create_time)
		if err != nil {
			panic(err)
		}

		arr = append(arr, s)
	}

	fmt.Println(arr)
}

func (r PgDemo) insert() {

	db := r.connDb()

	sql := " insert into t1(name, gender, create_time) values($1,$2,$3) "

	stmt, err := db.Prepare(sql)

	if err != nil {
		panic(err)
	}

	defer stmt.Close()

	c := time.Now().UnixNano() / 1e6

	s := Student{name: "wong", gender: "1", create_time: c}

	_, err = stmt.Exec(s.name, s.gender, s.create_time)

	if err != nil {
		panic(err)
	}

	fmt.Println("insert ok")
}

func (r PgDemo) update() {

	db := r.connDb()

	sql := " update t1 set gender = $1 where id = $2  "

	stmt, err := db.Prepare(sql)

	if err != nil {
		panic(err)
	}

	defer stmt.Close()

	_, err = stmt.Exec("2", "4")

	if err != nil {
		panic(err)
	}

	fmt.Println("update ok")
}

func (r PgDemo) delete() {

	db := r.connDb()

	sql := " delete from t1 where name = $1 "

	stmt, err := db.Prepare(sql)

	if err != nil {
		panic(err)
	}

	defer stmt.Close()

	_, err = stmt.Exec("wong")

	if err != nil {
		panic(err)
	}

	fmt.Println("delete ok")
}

type Student struct {
	id          int
	name        string
	gender      string
	create_time int64
}
