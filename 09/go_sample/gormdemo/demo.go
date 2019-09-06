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

	var s Student

	db.Debug().Where("name = ?", "catcher").First(&s)

	fmt.Println(s)

}

func (Student) TableName() string {
	return "t1"
}
