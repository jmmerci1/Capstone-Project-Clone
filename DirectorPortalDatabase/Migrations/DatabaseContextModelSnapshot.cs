// <auto-generated />
using System;
using DirectorPortalDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DirectorPortalDatabase.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11");

            modelBuilder.Entity("DirectorPortalDatabase.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("TEXT");

                    b.Property<int>("ZipCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ZipCodeExt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Business", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BusinessName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtraFields")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtraNotes")
                        .HasColumnType("TEXT");

                    b.Property<int>("MailingAddressId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MembershipLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PhysicalAddressId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Website")
                        .HasColumnType("TEXT");

                    b.Property<int>("YearEstablished")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MailingAddressId");

                    b.HasIndex("PhysicalAddressId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.BusinessRep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BusinessId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContactPersonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("BusinessReps");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Categories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BusinessId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.CategoryRef", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BusinessId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryRef");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.ContactPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ContactPeople");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContactPersonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.PhoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContactPersonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GEnumPhoneType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("PhoneNumbers");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.ReportField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModelPropertyName")
                        .HasColumnType("TEXT");

                    b.Property<int>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("ReportFields");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.ReportTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModelName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReportTemplateName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ReportTemplates");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("MarkedAsDone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TodoListItems");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.YearlyData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BallotNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BusinessId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Credit")
                        .HasColumnType("REAL");

                    b.Property<double>("DuesPaid")
                        .HasColumnType("REAL");

                    b.Property<string>("ExtraFields")
                        .HasColumnType("TEXT");

                    b.Property<int>("TermLength")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TicketsReturned")
                        .HasColumnType("REAL");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("BusinessYearlyData");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Business", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.Address", "MailingAddress")
                        .WithMany()
                        .HasForeignKey("MailingAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DirectorPortalDatabase.Models.Address", "PhysicalAddress")
                        .WithMany()
                        .HasForeignKey("PhysicalAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.BusinessRep", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DirectorPortalDatabase.Models.ContactPerson", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Categories", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.Business", null)
                        .WithMany("Categories")
                        .HasForeignKey("BusinessId");
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.CategoryRef", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DirectorPortalDatabase.Models.Categories", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.Email", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.ContactPerson", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.PhoneNumber", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.ContactPerson", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.ReportField", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.ReportTemplate", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DirectorPortalDatabase.Models.YearlyData", b =>
                {
                    b.HasOne("DirectorPortalDatabase.Models.Business", "Business")
                        .WithMany("YearlyData")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
