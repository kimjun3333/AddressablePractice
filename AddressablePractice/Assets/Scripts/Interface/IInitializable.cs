using System.Threading.Tasks;

/// <summary>
/// 매니저 클래스의 Init을 비동기로 처리해서 초기화 순서를 코드로 확인하고 제어하기 위함.
/// </summary>
public interface IInitializable
{
    Task Init();
}