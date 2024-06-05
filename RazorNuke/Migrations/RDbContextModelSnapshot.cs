﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RazorNuke.DbContext;

#nullable disable

namespace RazorNuke.Migrations
{
    [DbContext(typeof(RDbContext))]
    partial class RDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RSecurityBackend.Models.Audit.Db.REvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JsonData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResponseStatusCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.BannedEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BannedEmails");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RAppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RAppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LockoutMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("RImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ScreenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("SureName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("RImageId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RCaptchaImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RImageId");

                    b.ToTable("CaptchaImages");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OperationShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RAppRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SecurableItemShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RAppRoleId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RTemporaryUserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientAppName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientIPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastRenewal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RAppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RAppUserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RUserBehaviourLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserBehaviourLogs");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RVerifyQueueItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientAppName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientIPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QueueType")
                        .HasColumnType("int");

                    b.Property<string>("Secret")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Secret")
                        .IsUnique()
                        .HasFilter("[Secret] IS NOT NULL");

                    b.ToTable("VerifyQueueItems");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OperationShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RWSRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SecurableItemShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RWSRoleId");

                    b.ToTable("RWSPermissions");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("RWSRoles");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("InviteDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("MemberFrom")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("RAppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RWorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("WorkspaceOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RAppUserId");

                    b.HasIndex("RWorkspaceId");

                    b.ToTable("RWSUsers");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSUserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("RWSUserRoles");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWorkspace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RWorkspaces");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.WorkspaceUserInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("WorkspaceUserInvitations");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Generic.Db.RGenericOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("RAppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RAppUserId", "Name")
                        .IsUnique()
                        .HasFilter("[RAppUserId] IS NOT NULL AND [Name] IS NOT NULL");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Generic.Db.RLongRunningJobStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Progress")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Step")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Succeeded")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("LongRunningJobs");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Image.RImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("FileSizeInBytes")
                        .HasColumnType("bigint");

                    b.Property<string>("FolderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImageHeight")
                        .HasColumnType("int");

                    b.Property<int>("ImageWidth")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("OriginalFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StoredFileName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GeneralImages");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Notification.RUserNotification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("HtmlText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RAppUser", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Image.RImage", "RImage")
                        .WithMany()
                        .HasForeignKey("RImageId");

                    b.Navigation("RImage");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RCaptchaImage", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Image.RImage", "RImage")
                        .WithMany()
                        .HasForeignKey("RImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RImage");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RPermission", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppRole", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RAppRoleId");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RTemporaryUserSession", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "RAppUser")
                        .WithMany()
                        .HasForeignKey("RAppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RAppUser");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RUserBehaviourLog", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSPermission", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Cloud.RWSRole", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RWSRoleId");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSRole", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Cloud.RWorkspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSUser", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "RAppUser")
                        .WithMany()
                        .HasForeignKey("RAppUserId");

                    b.HasOne("RSecurityBackend.Models.Cloud.RWorkspace", null)
                        .WithMany("Members")
                        .HasForeignKey("RWorkspaceId");

                    b.Navigation("RAppUser");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSUserRole", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Cloud.RWSRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("RSecurityBackend.Models.Cloud.RWorkspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId");

                    b.Navigation("Role");

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.WorkspaceUserInvitation", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RSecurityBackend.Models.Cloud.RWorkspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId");

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Generic.Db.RGenericOption", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "RAppUser")
                        .WithMany()
                        .HasForeignKey("RAppUserId");

                    b.Navigation("RAppUser");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Notification.RUserNotification", b =>
                {
                    b.HasOne("RSecurityBackend.Models.Auth.Db.RAppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Auth.Db.RAppRole", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWSRole", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("RSecurityBackend.Models.Cloud.RWorkspace", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
