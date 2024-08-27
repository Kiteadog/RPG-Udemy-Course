using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.instance.PlaySFX(areaSoundIndex, null);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.instance.StopSFXWithTime(areaSoundIndex);//�˴�������뿪�������̽��봥��������Ч��BUG�������ڴ˴�ʵ��Э�̣�Ȼ���ڽ��봥������ĺ�������ֹͣЭ�̡�
    }
}
