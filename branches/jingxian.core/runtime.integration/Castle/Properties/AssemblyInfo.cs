using System.Reflection;
using System.Runtime.CompilerServices;

// �йس��򼯵ĳ�����Ϣ��ͨ������
// ���Լ����Ƶġ�������Щ����ֵ���޸������
// ��������Ϣ��

// TODO: �����򼯵�����ֵ

[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


// ���򼯵İ汾��Ϣ������ 4 ��ֵ���:
//
//      ���汾
//      �ΰ汾
//      �޶���
//      �ڲ��汾��
//
// ������ָ������ֵ��ʹ�á��޶��š��͡��ڲ��汾�š���Ĭ��ֵ��������
// ��������ʾʹ�� '*':

[assembly: AssemblyVersion("1.0.*")]

//
// Ϊ�˶Գ��򼯽���ǩ��������ָ��Ҫʹ�õ���Կ���йس���ǩ���ĸ�����Ϣ����ο�
// Microsoft .NET Framework �ĵ���
//
// ʹ���������Կ�������ǩ������Կ��
//
// ע��: 
//   (*) ���δָ����Կ�����޷��Գ��򼯽���ǩ����
//   (*) KeyName ��ָ�Ѿ���װ�ڼ�����ϵļ��ܷ���
//       �ṩ����(CSP)�е���Կ��KeyFile ��ָ������Կ��
//       �ļ���
//   (*) ���ͬʱָ���� KeyFile �� KeyName ֵ��������
//       ��ʽ���д���:
//       (1) ����� CSP �п����ҵ� KeyName����ʹ�ø���Կ��
//       (2) ��� KeyName �����ڶ� KeyFile ���ڣ��� KeyFile �� 
//           ����Կ��װ�� CSP �в�ʹ�ø���Կ��
//   (*) Ҫ���� KeyFile������ʹ�� sn.exe(ǿ����)ʵ�ù��ߡ�
//       ��ָ�� KeyFile ʱ��KeyFile ��λ��Ӧ�������
//       ��Ŀ���Ŀ¼����
//       %Project Directory%\obj\<configuration>�� ���磬��� KeyFile λ�ڸ�
//       ��ĿĿ¼�У�Ӧ�� AssemblyKeyFile 
//       ����ָ��Ϊ [assembly: AssemblyKeyFile("..\..\mykey.snk")]
//   (*)���ӳ�ǩ������һ���߼�ѡ��й����ĸ�����Ϣ������� Microsoft .NET Framework
//       �ĵ���
//

[assembly: AssemblyConfiguration("")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]


