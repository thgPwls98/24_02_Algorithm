using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Huffman
{
  public class HuffmanTree
  {
    private List<Node> nodes = new List<Node> ();
    public Node Root { get; set; }
    public Dictionary<char, int> Frequencies = new Dictionary<char, int> ();

    // 허프만 트리를 만든다
    public void Build(string source)
    {
      // 입력된 문자에서 Frequencies dictionary를 만든다
      for (int i = 0; i < source.Length; i++)
      {
        if (!Frequencies.ContainsKey(source[i]))
          Frequencies.Add(source[i], 0);
        Frequencies[source[i]]++;
      }

      // <Node> 리스트 nodes에 입력에서 추출한 문자들의 빈도를 저장한다
      // Node 클래스는 Symbol, Frequency, Left, Right로 구성된다
      foreach(KeyValuePair<char, int> kvp in Frequencies)
      {
        Node n = new Node() { Symbol=kvp.Key, Frequency=kvp.Value };
        nodes.Add(n);
      }

      while(nodes.Count > 1)
      {
        // List<Node> 타입의 리스트인 nodes를 node.Frequency 속성에 따라 오름차순으로 정렬한 후,
        // 그 결과를 새로운 리스트로 반환한다.

        // OrderBy는 LINQ(Language Integrated Query)에서 제공하는 메서드로,
        // 컬렉션을 특정 조건에 따라 정렬할 때 사용됩니다.
        // node => node.Frequency는 람다식으로,
        // 각 node 객체의 Frequency 속성을 기준으로 정렬하겠다는 뜻입니다.

        List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

        if(orderedNodes.Count >= 2)
        {
          // Take first two items
          List<Node> taken = orderedNodes.Take(2).ToList<Node>();

          // Create a parent node by combining the frequencies
          Node parent = new Node()
          {
            Symbol = '*',
            Frequency = taken[0].Frequency + taken[1].Frequency,
            Left = taken[0],
            Right = taken[1]
          };

          nodes.Remove(taken[0]);
          nodes.Remove(taken[1]);
          nodes.Add(parent);
        }

        // 첫번째 요소를 반환하거나 없으면 기본을 반환한다
        this.Root = nodes.FirstOrDefault();
      }
    }

    public BitArray Encode(string source)
    {
      List<bool> encodedSource = new List<bool>();

      for (int i = 0; i < source.Length; i++)
      {
        List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
        encodedSource.AddRange(encodedSymbol);
      }

      BitArray bits = new BitArray(encodedSource.ToArray());

      return bits;
    }

    public string Decode(BitArray bits)
    {
      Node current = this.Root;
      string decoded = "";

      foreach (bool bit in bits)
      {
        if (bit)
        {
          if (current.Right != null)
            current = current.Right;
        }
        else
        {
          if (current.Left != null)
            current = current.Left;
        }

        // 디코딩 하고 current를 Root로 초기화
        if (current.Left == null && current.Right == null) // Leaf
        {
          decoded += current.Symbol;
          current = this.Root;
        }
      }

      return decoded;
    }
  }
}