using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Models;

public partial class SeminarManagementDbContext : DbContext
{
    public SeminarManagementDbContext()
    {
    }

    public SeminarManagementDbContext(DbContextOptions<SeminarManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingTicket> BookingTickets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventSponsor> EventSponsors { get; set; }

    public virtual DbSet<Hall> Halls { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-JNCKUFJ;Initial Catalog=SeminarManagementDB;User ID=sa;Password=12345;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__5DE3A5B151102E64");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId)
                .ValueGeneratedNever()
                .HasColumnName("booking_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__user_id__4D94879B");
        });

        modelBuilder.Entity<BookingTicket>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BookingTicket");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");

            entity.HasOne(d => d.Booking).WithMany()
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__BookingTi__booki__4E88ABD4");

            entity.HasOne(d => d.Ticket).WithMany()
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__BookingTi__ticke__4F7CD00D");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4F75CFBA0");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__2370F7274C73B244");

            entity.ToTable("Event");

            entity.Property(e => e.EventId)
                .ValueGeneratedNever()
                .HasColumnName("event_id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("creation_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.EventCode)
                .HasMaxLength(255)
                .HasColumnName("eventCode");
            entity.Property(e => e.EventName)
                .HasMaxLength(255)
                .HasColumnName("eventName");
            entity.Property(e => e.Fee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fee");
            entity.Property(e => e.HallId).HasColumnName("hall_id");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.QrCode).HasColumnName("qrCode");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.Hall).WithMany(p => p.Events)
                .HasForeignKey(d => d.HallId)
                .HasConstraintName("FK__Event__hall_id__5070F446");
        });

        modelBuilder.Entity<EventSponsor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EventSponsor");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.SponsorId).HasColumnName("sponsor_id");

            entity.HasOne(d => d.Event).WithMany()
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__EventSpon__event__5165187F");

            entity.HasOne(d => d.Sponsor).WithMany()
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__EventSpon__spons__52593CB8");
        });

        modelBuilder.Entity<Hall>(entity =>
        {
            entity.HasKey(e => e.HallId).HasName("PK__Hall__A63DE8CF6837BDA8");

            entity.ToTable("Hall");

            entity.Property(e => e.HallId)
                .ValueGeneratedNever()
                .HasColumnName("hall_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.HallDescription).HasColumnName("hall_description");
            entity.Property(e => e.HallName)
                .HasMaxLength(255)
                .HasColumnName("hall_name");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC88B83B11");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.SponsorId).HasName("PK__Sponsor__BE37D4547421CA11");

            entity.ToTable("Sponsor");

            entity.Property(e => e.SponsorId)
                .ValueGeneratedNever()
                .HasColumnName("sponsor_id");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.SponsorName)
                .HasMaxLength(255)
                .HasColumnName("sponsor_name");
            entity.Property(e => e.SponsorType)
                .HasMaxLength(255)
                .HasColumnName("sponsor_type");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.SurveyId).HasName("PK__Survey__9DC31A07298A8680");

            entity.ToTable("Survey");

            entity.Property(e => e.SurveyId)
                .ValueGeneratedNever()
                .HasColumnName("survey_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.FeedBackContent).HasColumnName("feedBackContent");
            entity.Property(e => e.FinishDate)
                .HasColumnType("datetime")
                .HasColumnName("finishDate");

            entity.HasOne(d => d.Event).WithMany(p => p.Surveys)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Survey__event_id__534D60F1");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__D596F96BDE59DA78");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId)
                .ValueGeneratedNever()
                .HasColumnName("ticket_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Category).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Ticket__category__5441852A");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Ticket__event_id__5535A963");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AFF38B9A5C");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("creation_date");
            entity.Property(e => e.DepositAmount)
                .HasColumnType("money")
                .HasColumnName("deposit_amount");
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(255)
                .HasColumnName("transaction_status");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WalletId).HasColumnName("wallet_id");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Transaction");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_Wallet_Transaction");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370FADC8C173");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__F3DBC572B07629FE").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("firstName");
            entity.Property(e => e.IsActivated).HasColumnName("isActivated");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.IssueTokenDate)
                .HasColumnType("datetime")
                .HasColumnName("issue_token_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("lastName");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phone_number");
            entity.Property(e => e.QrCode).HasColumnName("qrCode");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
            entity.Property(e => e.VerifyToken).HasColumnName("verify_token");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__role_id__5812160E");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Wallet__0EE6F041FCCD7334");

            entity.ToTable("Wallet");

            entity.Property(e => e.WalletId)
                .ValueGeneratedNever()
                .HasColumnName("wallet_id");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("balance");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
