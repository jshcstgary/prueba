using Microsoft.EntityFrameworkCore;

using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public virtual DbSet<Person> Persons { get; set; }

	public virtual DbSet<RoleOption> RoleOptions { get; set; }

	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<Session> Sessions { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Person>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Persons");

			entity.ToTable("PERSONS");

			entity.HasIndex(e => e.Identification, "IX__Persons__Identification").IsUnique();

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.BirthDate)
				.HasColumnType("datetime")
				.HasColumnName("birth_date");
			entity.Property(e => e.Identification)
				.HasMaxLength(10)
				.HasColumnName("identification");
			entity.Property(e => e.Names)
				.HasMaxLength(60)
				.HasColumnName("names");
			entity.Property(e => e.Surnames)
				.HasMaxLength(60)
				.HasColumnName("surnames");
		});

		modelBuilder.Entity<RoleOption>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Role_Options");

			entity.ToTable("ROLE_OPTIONS");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Name)
				.HasMaxLength(50)
				.HasColumnName("name");
			entity.Property(e => e.Status)
				.HasMaxLength(20)
				.IsUnicode(false)
				.IsFixedLength()
				.HasColumnName("status");
		});

		modelBuilder.Entity<Role>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Roles");

			entity.ToTable("ROLES");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.Name)
				.HasMaxLength(50)
				.HasColumnName("name");
			entity.Property(e => e.Status)
				.HasMaxLength(20)
				.IsUnicode(false)
				.IsFixedLength()
				.HasColumnName("status");

			entity.HasMany(d => d.RoleOptions).WithMany(p => p.IdRoles)
				.UsingEntity<Dictionary<string, object>>(
					"RolesRoleRoption",
					r => r.HasOne<RoleOption>().WithMany()
						.HasForeignKey("IdRoleOption")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Roles__Role_Options__Role_Options"),
					l => l.HasOne<Role>().WithMany()
						.HasForeignKey("IdRole")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Roles__Role_Options__Roles"),
					j =>
					{
						j.HasKey("IdRole", "IdRoleOption").HasName("PK__Roles__Role_Options");
						j.ToTable("ROLES_ROLE_OPTIONS");
						j.IndexerProperty<int>("IdRole").HasColumnName("id_role");
						j.IndexerProperty<int>("IdRoleOption").HasColumnName("id_role_option");
					});

			entity.HasMany(d => d.Users).WithMany(p => p.IdRoles)
				.UsingEntity<Dictionary<string, object>>(
					"RolesUser",
					r => r.HasOne<User>().WithMany()
						.HasForeignKey("IdUser")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Roles_Users__Users"),
					l => l.HasOne<Role>().WithMany()
						.HasForeignKey("IdRole")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Roles_Users__Roles"),
					j =>
					{
						j.HasKey("IdRole", "IdUser").HasName("PK__Roles__Users");
						j.ToTable("ROLES_USERS");
						j.IndexerProperty<int>("IdRole").HasColumnName("id_role");
						j.IndexerProperty<int>("IdUser").HasColumnName("id_user");
					});
		});

		modelBuilder.Entity<Session>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Sessions");

			entity.ToTable("SESSIONS");

			entity.Property(e => e.Id).HasColumnName("id");
			entity.Property(e => e.CloseDate)
				.HasColumnType("datetime")
				.HasColumnName("close_date");
			entity.Property(e => e.EntryDate)
				.HasColumnType("datetime")
				.HasColumnName("entry_date");
			entity.Property(e => e.Status)
				.HasMaxLength(15)
				.HasColumnName("status");
			entity.Property(e => e.IdUser).HasColumnName("id_user");

			entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Sessions)
				.HasForeignKey(d => d.IdUser)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Sessions__Users");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Users");

			entity.ToTable("USERS");

			entity.HasIndex(e => e.Mail, "IX__Users__Mail").IsUnique();

			entity.HasIndex(e => e.Username, "IX__Users__Username").IsUnique();

			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd()
				.HasColumnName("id");
			entity.Property(e => e.IdPerson).HasColumnName("id_person");
			entity.Property(e => e.Mail)
				.HasMaxLength(120)
				.HasColumnName("mail");
			entity.Property(e => e.Password)
				.HasMaxLength(50)
				.HasColumnName("password");
			entity.Property(e => e.SessionActive)
				.HasColumnType("bit")
				.HasColumnName("session_active");
			entity.Property(e => e.Status)
				.HasMaxLength(20)
				.IsUnicode(false)
				.IsFixedLength()
				.HasColumnName("status");
			entity.Property(e => e.Username)
				.HasMaxLength(20)
				.HasColumnName("username");

			entity.HasOne(d => d.IdNavigation).WithOne(p => p.User)
				.HasForeignKey<User>(d => d.IdPerson)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Users__Persons");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
