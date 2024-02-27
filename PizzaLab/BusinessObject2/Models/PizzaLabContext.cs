using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Models;

public partial class PizzaLabContext : DbContext
{
	public PizzaLabContext()
	{
	}

	public PizzaLabContext(DbContextOptions<PizzaLabContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Order> Orders { get; set; }

	public virtual DbSet<OrderDetail> OrderDetails { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<Review> Reviews { get; set; }

	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		//ref to current directory app
		IConfigurationRoot configuration = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: true)
		.Build();
		optionsBuilder.UseSqlServer(configuration["ConnectionStrings:PizzaLabDB"]);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>(entity =>
		{
			entity.ToTable("Category");

			entity.Property(e => e.CategoryName)
				.IsRequired()
				.HasMaxLength(50);
			entity.Property(e => e.Description).HasMaxLength(250);
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.ToTable("Order");

			entity.Property(e => e.Freight).HasColumnType("money");
			entity.Property(e => e.IsCart).HasColumnName("isCart");
			entity.Property(e => e.OrderDate).HasColumnType("datetime");
			entity.Property(e => e.RequiredDate).HasColumnType("datetime");
			entity.Property(e => e.ShipAddress)
				.IsRequired()
				.HasMaxLength(50);
			entity.Property(e => e.ShippedDate).HasColumnType("datetime");

			entity.HasOne(d => d.User).WithMany(p => p.Orders)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Order_User");
		});

		modelBuilder.Entity<OrderDetail>(entity =>
		{
			entity.HasKey(e => new { e.OrderId, e.ProductId });

			entity.ToTable("OrderDetail");

			entity.Property(e => e.Price).HasColumnType("money");

			entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.OrderId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_OrderDetail_Order");

			entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
				.HasForeignKey(d => d.ProductId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_OrderDetail_Product");
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.ToTable("Product");

			entity.Property(e => e.Description).IsRequired();
			entity.Property(e => e.Image)
				.IsRequired()
				.IsUnicode(false);
			entity.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(50);
			entity.Property(e => e.Price).HasColumnType("money");

			entity.HasOne(d => d.Category).WithMany(p => p.Products)
				.HasForeignKey(d => d.CategoryId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Product_Category");
		});

		modelBuilder.Entity<Review>(entity =>
		{
			entity.ToTable("Review");

			entity.Property(e => e.Comment).IsRequired();
			entity.Property(e => e.DateModified).HasColumnType("datetime");

			entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
				.HasForeignKey(d => d.ProductId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Review_Product");

			entity.HasOne(d => d.User).WithMany(p => p.Reviews)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Review_User");
		});

		modelBuilder.Entity<Role>(entity =>
		{
			entity.ToTable("Role");

			entity.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(50);
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("User");

			entity.Property(e => e.ContactName).HasMaxLength(50);
			entity.Property(e => e.Email)
				.IsRequired()
				.HasMaxLength(50);
			entity.Property(e => e.FirstName).HasMaxLength(50);
			entity.Property(e => e.LastName).HasMaxLength(50);
			entity.Property(e => e.Password)
				.IsRequired()
				.HasMaxLength(18);
			entity.Property(e => e.Phone)
				.HasMaxLength(10)
				.IsFixedLength();

			entity.HasOne(d => d.Role).WithMany(p => p.Users)
				.HasForeignKey(d => d.RoleId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_User_Role");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
