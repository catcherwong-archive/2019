package gormdemo

import (
	"github.com/jinzhu/gorm"

	_ "github.com/jinzhu/gorm/dialects/postgres"

	"fmt"
)

type Student struct {
	Id         int    `gorm:"column:id"`
	Name       string `gorm:"column:name"`
	Gender     string `gorm:"column:gender"`
	CreateTime int64  `gorm:"column:create_time"`
}

func Show() {

	db, err := gorm.Open("postgres", "host=localhost user=postgres dbname=demo sslmode=disable password=123456")
	if err != nil {
		panic("can not open database")
	}
	defer db.Close()

	fmt.Println(db.HasTable("t1"))

	// // find all
	// var arr []Student
	// db.Debug().Find(&arr)
	// fmt.Println(arr)

	// // limit offset
	// var arr1 []Student
	// db.Debug().Offset(1).Limit(2).Find(&arr1)
	// fmt.Println(arr1)

	// // find by name
	// var s Student
	// db.Debug().Where("name = ?", "catcher").First(&s)
	// fmt.Println(s)

	// // insert
	// s1 := Student{Name: "wong", Gender: "1", CreateTime: time.Now().UnixNano() / 1e6}
	// db.Debug().Create(&s1)
	// db.Find(&arr)
	// fmt.Println(arr)

	// var s2 Student
	// db.Debug().Model(&s2).Where("name = ?", "wong").Update("gender", "2")
	// db.Find(&arr)
	// fmt.Println(arr)

	// db.Debug().Where("name = ?", "wong").Delete(Student{})
	// db.Find(&arr)
	// fmt.Println(arr)

	var n string
	var t int64
	r := db.Raw(`SELECT name, create_time FROM "t1"  WHERE name = ? limit 1`, "catcher").Row()
	r.Scan(&n, &t)

	fmt.Println(n, t)
}

func (Student) TableName() string {
	return "t1"
}
