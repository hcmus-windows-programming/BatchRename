# BatchRename

## Thông tin nhóm

1. 18120321 - Huỳnh Thanh Đức
2. 19120590 - Huỳnh Thanh Mỹ
3. 20120046 - Ngô Xuân Chiến
4. 20120073 - Văn Lý Hải
5. 20120293 - Võ Phi Hùng

## Các chức năng cơ bản đã hoàn thành

1. Dynamically load all renaming rules from external DLL files
2. Select all files and folders you want to rename
3. Create a set of rules for renaming the files.
   - Each rule can be added from a menu
   - Each rule's parameters can be edited
4. Apply the set of rules in numerical order to each file, make them have a new name
5. Save this set of rules into presets for quickly loading later if you need to reuse

## Các chức năng cơ bản chưa hoàn thành

- Không có

## Các chức năng làm thêm

- Let the user see the preview of the result

## Chạy source code

- Yêu cầu sử dụng Visual Studio 2022 và .NET 7.0
- Mở `DynamicBatchRename.sln` để build lại các file dll (nếu có update rule)
- Copy các file dll này vào folder `DLLFiles`
- Mở `BatchRename.sln` và chạy chương trình

## Video demo

- [Demo BatchRename](https://youtu.be/gl0SbGCeKbg)
