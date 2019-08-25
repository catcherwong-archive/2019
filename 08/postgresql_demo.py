# -*- coding: UTF-8 -*-

import psycopg2  #postgresql
import time
import datetime 

class PgDemo:

    def __init__(self, host, port, db, user, pwd):
        self.host = host
        self.port = port
        self.db = db
        self.user = user
        self.pwd = pwd
            
    def getConnection(self):

        conn = None
        try:
            conn = psycopg2.connect(
                host=self.host,
                port=self.port,
                database=self.db,
                user=self.user,
                password=self.pwd,             
            )
        except Exception as err:
            print("can not connect to the database，%s" % err)
        
        return conn
      

    def query_all(self):

        with self.getConnection() as conn:            
            sql = "select id, name, gender, create_time from t1"
            try:
                cur = conn.cursor()
                cur.execute(sql)
                res = cur.fetchall()
                # print(res)
                print("id\tname\tgender\ttime")
                for d in res:                    
                    print("%d\t%s\t%s\t%s" % (d[0], d[1], "male"  if d[2] == 1 else "female",  self.timestamp2datetime(d[3], False)))
            except Exception as err:
                print("query all fail, %s" % err)        
            finally:
                cur.close()

    def query_lastone(self):

        with self.getConnection() as conn:              
            sql = "select id, name, gender, create_time from t1 order by create_time desc limit 1"
            try:
                cur = conn.cursor()
                cur.execute(sql)
                res = cur.fetchone()
                # print(res)
                print("id\tname\tgender\ttime")
                print("%d\t%s\t%s\t%s" % (res[0], res[1], "male"  if res[2] == 1 else "female",  self.timestamp2datetime(res[3], False)))
            except Exception as err:
                print("query lastone fail, %s" % err)        
            finally:
                cur.close()         

    def query_byname(self, name):

        with self.getConnection() as conn:              
            sql = "select id, name, gender, create_time from t1 where name = %s"
            try:
                cur = conn.cursor()
                cur.execute(sql, (name, ))
                res = cur.fetchone()
                # print(res)
                print("id\tname\tgender\ttime")
                print("%d\t%s\t%s\t%s" % (res[0], res[1], "male"  if res[2] == 1 else "female",  self.timestamp2datetime(res[3], False)))
            except Exception as err:
                print("query by name fail, %s" % err)        
            finally:
                cur.close()         

    def insert_one(self, name, gender):

        with self.getConnection() as conn:              
            sql = " insert into t1(name, gender, create_time) values(%s, %s, %s) "
            try:                
                cur = conn.cursor()                
                cur.execute(sql, (name, gender,  self.getCurrentTimestamp()))     
                print("insert ok")          
            except Exception as err:
                print("insert one fail, %s" % err)        
            finally:
                cur.close()                             

    def update_genderbyid(self, id, gender):

        with self.getConnection() as conn:              
            sql = " update t1 set gender = %s where id = %s "
            try:                
                cur = conn.cursor()                
                cur.execute(sql, (gender,  id))     
                print("update ok")          
            except Exception as err:
                print("update gender by id fail, %s" % err)        
            finally:
                cur.close()       

    def delete_byname(self, name):

        with self.getConnection() as conn:              
            sql = " delete from t1 where name = %s "
            try:                
                cur = conn.cursor()                
                cur.execute(sql, (name,  ))     
                print("delete ok")          
            except Exception as err:
                print("delete by name fail, %s" % err)        
            finally:
                cur.close()                    

    def getCurrentTimestamp(self):
        ts = int ( round ( time.time() * 1000 ) )
        print(ts)
        return ts
        
    def timestamp2datetime(self, timestamp, issecond):
        if(issecond == True):
            t = datetime.datetime.fromtimestamp(timestamp)
            return t.strftime("%Y-%m-%d %H:%M:%S")
        else:
            t = datetime.datetime.fromtimestamp(timestamp / 1000)
            return t.strftime("%Y-%m-%d %H:%M:%S.%f")[:-3]

if __name__ == "__main__":
    pg = PgDemo("127.0.0.1", 5432, "demo", "postgres", "123456")

    print("===========insert_one==============")
    pg.insert_one("wong", 1)

    print("===========query_all==============")    
    pg.query_all()

    print("===========query_lastone==============")
    pg.query_lastone()

    print("===========query_byname==============")
    pg.query_byname("catcher")

    print("===========update_genderbyid==============")
    pg.update_genderbyid(4, 2)

    print("===========delete_byname==============")
    pg.delete_byname("wong")

    print("===========query_all==============")    
    pg.query_all()
        