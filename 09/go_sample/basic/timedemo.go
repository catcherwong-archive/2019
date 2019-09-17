package basic

import (
	"fmt"
	"strings"
	"time"
)

type TimeDemo struct {
}

func ShowTime() {

	t := new(TimeDemo)

	t.formatString()

	t.timestamp2String()
}

func (t TimeDemo) formatString() {

	n := time.Now()

	// C# format
	fmt.Println(TimeToString(n, "yy-MM-dd HH:mm:ss"))
	fmt.Println(TimeToString(n, "yyyy-MM-dd HH:mm:ss"))
	fmt.Println(TimeToString(n, "yyyy-MM-dd hh:mm:ss"))

	fmt.Println(TimeToString(n, "yyyy-MM-dd HH:mm:ss.fff"))
	fmt.Println(TimeToString(n, "yyyy-MM-dd HH:mm:ss.ffffff"))
}

func (t TimeDemo) getTimestamp() {

	n := time.Now()

	// now()
	fmt.Println(TimeToUnixTimestamp(n, 0))
	// now(3)
	fmt.Println(TimeToUnixTimestamp(n, 3))
	// now(6)
	fmt.Println(TimeToUnixTimestamp(n, 6))
	// now(9)
	fmt.Println(TimeToUnixTimestamp(n, 9))

	fmt.Println(TimeToUnixTimestamp(n, 5))
}

func (t TimeDemo) timestamp2String() {

	n := time.Now()

	st := n.Unix()

	fmt.Printf("second of unix timestamp is %d\n", st)

	t1 := UnixTimestampToString(st, true)

	fmt.Println(t1)

	mt := n.UnixNano() / int64(time.Millisecond)

	fmt.Printf("millisecond of unix timestamp is %d\n", mt)

	t2 := UnixTimestampToString(mt, false)

	fmt.Println(t2)

}

func TimeToString(t time.Time, format string) string {
	res := strings.Replace(format, "yyyy", t.Format("2006"), -1)
	res = strings.Replace(res, "yy", t.Format("06"), -1)
	res = strings.Replace(res, "MM", t.Format("01"), -1)
	res = strings.Replace(res, "M", t.Format("1"), -1)
	res = strings.Replace(res, "dd", t.Format("02"), -1)
	res = strings.Replace(res, "d", t.Format("2"), -1)
	res = strings.Replace(res, "HH", fmt.Sprintf("%02d", t.Hour()), -1)
	res = strings.Replace(res, "H", fmt.Sprintf("%d", t.Hour()), -1)
	res = strings.Replace(res, "hh", t.Format("03"), -1)
	res = strings.Replace(res, "h", t.Format("3"), -1)
	res = strings.Replace(res, "mm", t.Format("04"), -1)
	res = strings.Replace(res, "m", t.Format("4"), -1)
	res = strings.Replace(res, "ss", t.Format("05"), -1)
	res = strings.Replace(res, "s", t.Format("5"), -1)
	res = strings.Replace(res, "ffffff", fmt.Sprintf("%6d", t.Nanosecond()/int(time.Microsecond)), -1)
	res = strings.Replace(res, "fffff", fmt.Sprintf("%5d", t.Nanosecond()/int(time.Microsecond*10)), -1)
	res = strings.Replace(res, "ffff", fmt.Sprintf("4%d", t.Nanosecond()/int(time.Microsecond*100)), -1)
	res = strings.Replace(res, "fff", fmt.Sprintf("%3d", t.Nanosecond()/int(time.Millisecond)), -1)
	res = strings.Replace(res, "ff", fmt.Sprintf("%2d", t.Nanosecond()/int(time.Millisecond*10)), -1)
	res = strings.Replace(res, "f", fmt.Sprintf("%d", t.Nanosecond()/int(time.Millisecond*100)), -1)

	return res
}

func UnixTimestampToString(ti int64, isSec bool) string {

	var s string

	if isSec {
		s = time.Unix(ti, 0).Format("2006-01-02 15:04:05")
	} else {
		s = time.Unix(0, ti).Format("2006-01-02 15:04:05.000")
	}

	return s
}

func TimeToUnixTimestamp(t time.Time, digit int) int64 {

	var i int64

	switch digit {
	case 0:
		i = t.Unix()
	case 3:
		i = t.UnixNano() / int64(time.Millisecond)
	case 6:
		i = t.UnixNano() / int64(time.Microsecond)
	case 9:
		i = t.UnixNano()
	default:
		i = t.Unix()
	}

	return i
}
