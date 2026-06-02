package widgetario.products;

import java.io.Serializable;
import java.math.BigDecimal;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "products")
public class Product implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;

    @Column(name = "name")
    private String name;

    @Column(name = "price")
    private BigDecimal price;

    public Product() {}

    public Product(String name, BigDecimal price) {
        setName(name);
        setPrice(price);
    }

    public long getId() {
    	return id;
    }
    
    public void setId(long id) {
        this.id = id;
    }

    public String getName() {
    	return name;
    }
    
    public void setName(String name) {
        this.name = name;
    }

    public BigDecimal getPrice() {
    	return price;
    }
    
    public void setPrice(BigDecimal price) {
        this.price = price;
    }
}
